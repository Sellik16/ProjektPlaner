using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProjektPlaner.Data;
using ProjektPlaner.Models;

namespace ProjektPlaner.Controllers
{
    public class CalendarElementsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CalendarElementsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: CalendarElements
        public async Task<IActionResult> Index()
        {
            string currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(currentUserId))
            {
                return View(new List<CalendarElement>());
            }

            var userGroupIds = await _context.CalendarGroup
                .Where(g => g.FounderId == currentUserId
                         || g.Users.Any(u => u.Id == currentUserId)
                         || g.Administrators.Any(a => a.Id == currentUserId))
                .Select(g => g.Id)
                .ToListAsync();

            var elements = await _context.CalendarElement
                .Include(e => e.Group)
                .Include(e => e.User)
                .Where(e => e.UserId == currentUserId
                         || (e.GroupId.HasValue && userGroupIds.Contains(e.GroupId.Value)))
                .ToListAsync();

            return View(elements);
        }

        // GET: CalendarElements/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var calendarElement = await _context.CalendarElement
                .Include(c => c.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (calendarElement == null)
            {
                return NotFound();
            }

            return View(calendarElement);
        }

        public async Task<IActionResult> Create()
        {
            string currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userGroups = await _context.CalendarGroup
                .Include(g => g.Users)
                .Include(g => g.Administrators)
                .Where(g => g.Users.Any(u => u.Id == currentUserId)
                         || g.Administrators.Any(a => a.Id == currentUserId)
                         || g.FounderId == currentUserId)
                .ToListAsync();

            var groupSelectList = new List<SelectListItem>
                {
                    new SelectListItem("No Group", "0")
                };
            groupSelectList.AddRange(userGroups.Select(g => new SelectListItem(g.Name, g.Id.ToString())));

            ViewData["GroupId"] = groupSelectList;

            return View();
        }

        // POST: CalendarElements/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Description,StartDate,EndDate,Location,GroupId")] CalendarElement calendarElement)
        {
            string currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(currentUserId))
            {
                ModelState.AddModelError("", "You must be logged in to create a calendar element.");
                await PopulateGroupDropdown(currentUserId, calendarElement.GroupId);
                return View(calendarElement);
            }
            var currentUser = await _context.Users.FindAsync(currentUserId);
            if (currentUser == null)
            {
                ModelState.AddModelError("", "Current user not found in the database.");
                await PopulateGroupDropdown(currentUserId, calendarElement.GroupId);
                return View(calendarElement);
            }

            if (calendarElement.GroupId == 0)
            {
                calendarElement.GroupId = null;
            }

            if (calendarElement.GroupId.HasValue)
            {
                var selectedGroup = await _context.CalendarGroup
                    .Include(g => g.Users)
                    .Include(g => g.Administrators)
                    .FirstOrDefaultAsync(g => g.Id == calendarElement.GroupId.Value);

                if (selectedGroup == null)
                {
                    ModelState.AddModelError("", "The selected group does not exist.");
                }
                else if (
                    !selectedGroup.Users.Any(u => u.Id == currentUserId) &&
                    !selectedGroup.Administrators.Any(a => a.Id == currentUserId) &&
                    selectedGroup.FounderId != currentUserId
                )
                {
                    ModelState.AddModelError("", "You are not authorized to add elements to this group.");
                }
            }

            if (!ModelState.IsValid)
            {
                await PopulateGroupDropdown(currentUserId, calendarElement.GroupId);
                return View(calendarElement);
            }
            else
            {
                calendarElement.UserId = currentUserId;

                _context.Add(calendarElement);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            
        }

        private async Task PopulateGroupDropdown(string currentUserId, int? selectedGroupId)
        {
            var userGroups = await _context.CalendarGroup
                .Include(g => g.Users)
                .Include(g => g.Administrators)
                .Where(g => g.Users.Any(u => u.Id == currentUserId)
                         || g.Administrators.Any(a => a.Id == currentUserId)
                         || g.FounderId == currentUserId)
                .ToListAsync();

            var groupSelectList = new List<SelectListItem>
    {
        new SelectListItem("No Group", "0")
    };
            groupSelectList.AddRange(userGroups.Select(g => new SelectListItem(g.Name, g.Id.ToString())));

            var selectedValue = selectedGroupId.HasValue ? selectedGroupId.Value.ToString() : "0";
            ViewData["GroupId"] = new SelectList(groupSelectList, "Value", "Text", selectedValue);
        }

        // GET: CalendarElements/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var calendarElement = await _context.CalendarElement.FindAsync(id);
            if (calendarElement == null)
            {
                return NotFound();
            }
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", calendarElement.UserId);
            return View(calendarElement);
        }

        // POST: CalendarElements/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description,StartDate,EndDate,Location,Recurrence,UserId,GroupId")] CalendarElement calendarElement)
        {
            if (id != calendarElement.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(calendarElement);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CalendarElementExists(calendarElement.Id))
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
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", calendarElement.UserId);
            return View(calendarElement);
        }

        // GET: CalendarElements/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var calendarElement = await _context.CalendarElement
                .Include(c => c.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (calendarElement == null)
            {
                return NotFound();
            }

            return View(calendarElement);
        }

        // POST: CalendarElements/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var calendarElement = await _context.CalendarElement.FindAsync(id);
            if (calendarElement != null)
            {
                _context.CalendarElement.Remove(calendarElement);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CalendarElementExists(int id)
        {
            return _context.CalendarElement.Any(e => e.Id == id);
        }
        }
}
