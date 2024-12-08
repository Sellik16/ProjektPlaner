using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProjektPlaner.Data;
using ProjektPlaner.Models;

namespace ProjektPlaner.Controllers
{
    [Authorize]
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
            string FounderId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return View(await _context.CalendarGroup
                .Include(c => c.Founder)
                .Where(c => c.FounderId == FounderId)
                .ToListAsync());
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
            ViewData["FounderId"] = new SelectList(_context.Users, "Id", "Id");
            return View();
        }

        // POST: CalendarGroups/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Description")] CalendarGroup calendarGroup)
        {
            calendarGroup.FounderId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (ModelState.IsValid)
            {
                _context.Add(calendarGroup);
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
            ViewData["FounderId"] = new SelectList(_context.Users, "Id", "Id", calendarGroup.FounderId);
            return View(calendarGroup);
        }

        // POST: CalendarGroups/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description,FounderId")] CalendarGroup calendarGroup)
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
    }
}
