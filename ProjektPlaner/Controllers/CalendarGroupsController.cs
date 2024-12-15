using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProjektPlaner.Data;
using ProjektPlaner.Models;

namespace ProjektPlaner.Controllers
{
    public class CalendarGroupsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CalendarGroupsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: CalendarGroups
        public async Task<IActionResult> Index()
        {
            string currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var groups = await _context.CalendarGroup
                .Include(c => c.Users)
                .Include(c => c.Administrators)
                .Where(c => c.FounderId == currentUserId || c.Users.Any(u => u.Id == currentUserId))
                .ToListAsync();

            return View(groups);
        }

        // GET: CalendarGroups/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var calendarGroup = await _context.CalendarGroup
                .Include(c => c.Founder)
                .Include(c => c.Users)
                .Include(c => c.Administrators)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (calendarGroup == null)
            {
                return NotFound();
            }

            return View(calendarGroup);
        }

        // GET: CalendarGroups/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: CalendarGroups/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Description")] CalendarGroup calendarGroup)
        {
            string currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(currentUserId))
            {
                ModelState.AddModelError("", "You must be logged in to create a group.");
                return View(calendarGroup);
            }

            calendarGroup.FounderId = currentUserId;

            var currentUser = await _context.Users.FindAsync(currentUserId);
            if (currentUser == null)
            {
                ModelState.AddModelError("", "Current user not found in the database.");
                return View(calendarGroup);
            }

            calendarGroup.Users = calendarGroup.Users ?? new List<IdentityUser>();
            calendarGroup.Administrators = calendarGroup.Administrators ?? new List<IdentityUser>();

            calendarGroup.Users.Add(currentUser);
            calendarGroup.Administrators.Add(currentUser);

            if (ModelState.IsValid)
            {
                _context.CalendarGroup.Add(calendarGroup);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(calendarGroup);
        }

        // GET: CalendarGroups/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var calendarGroup = await _context.CalendarGroup.FindAsync(id);
            if (calendarGroup == null)
            {
                return NotFound();
            }
            ViewData["FounderId"] = new SelectList(_context.Users, "Id", "Email", calendarGroup.FounderId);
            return View(calendarGroup);
        }

        // POST: CalendarGroups/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description,FounderId,UsersId,AdministratorsId")] CalendarGroup calendarGroup)
        {
            if (id != calendarGroup.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(calendarGroup);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CalendarGroupExists(calendarGroup.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["FounderId"] = new SelectList(_context.Users, "Id", "Id", calendarGroup.FounderId);
            return View(calendarGroup);
        }

        // GET: CalendarGroups/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var calendarGroup = await _context.CalendarGroup
                .Include(c => c.Founder)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (calendarGroup == null)
            {
                return NotFound();
            }

            return View(calendarGroup);
        }

        // POST: CalendarGroups/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var calendarGroup = await _context.CalendarGroup.FindAsync(id);
            if (calendarGroup != null)
            {
                _context.CalendarGroup.Remove(calendarGroup);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CalendarGroupExists(int id)
        {
            return _context.CalendarGroup.Any(e => e.Id == id);
        }

        public async Task<IActionResult> AddUser(int? Id)
        {
            if (Id == null)
            {
                return NotFound();
            }

            var calendarGroup = await _context.CalendarGroup
                .Include(c => c.Users)
                .FirstOrDefaultAsync(c => c.Id == Id);

            if (calendarGroup == null)
            {
                return NotFound();
            }

            var allUsers = await _context.Users.ToListAsync();
            var existingUserIds = calendarGroup.Users.Select(u => u.Id).ToHashSet();
            var availableUsers = allUsers.Where(u => !existingUserIds.Contains(u.Id)).ToList();

            if (availableUsers.Count == 0)
            {
                ViewBag.Message = "All users are already members of this group.";
            }

            ViewData["UserId"] = new SelectList(availableUsers, "Id", "Email");
            ViewBag.GroupId = Id;

            return View();
        }

        // POST: CalendarGroups/AddUser
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddUser(int Id, string userId)
        {
            if (string.IsNullOrEmpty(userId) || Id == 0)
            {
                ModelState.AddModelError("", "Invalid user or group selection.");
                return RedirectToAction(nameof(Details), new { id = Id });
            }

            var calendarGroup = await _context.CalendarGroup
                .Include(c => c.Users)
                .FirstOrDefaultAsync(c => c.Id == Id);

            if (calendarGroup == null)
            {
                return NotFound();
            }

            var userToAdd = await _context.Users.FindAsync(userId);
            if (userToAdd == null)
            {
                ModelState.AddModelError("", "Selected user not found.");
                return RedirectToAction(nameof(Details), new { id = Id });
            }

            if (calendarGroup.Users == null)
            {
                calendarGroup.Users = new List<IdentityUser>();
            }

            if (!calendarGroup.Users.Any(u => u.Id == userId))
            {
                calendarGroup.Users.Add(userToAdd);
                _context.Update(calendarGroup);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Details), new { id = Id });
        }

    }
}
