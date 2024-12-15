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
            string FounderId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return View(await _context.CalendarGroup
                .Include(c => c.Founder)
                .Where(c => c.FounderId == FounderId)
                .ToListAsync());
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

        // GET: CalendarElements/Create
        public IActionResult Create()
        {
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id");
            return View();
        }

        // POST: CalendarElements/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Description,StartDate,EndDate,Location,Recurrence,UserId,GroupId")] CalendarElement calendarElement)
        {
            if (ModelState.IsValid)
            {
                _context.Add(calendarElement);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", calendarElement.UserId);
            return View(calendarElement);
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
