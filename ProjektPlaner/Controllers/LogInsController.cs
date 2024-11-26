using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProjektPlaner.Data;
using ProjektPlaner.Models;

namespace ProjektPlaner.Controllers
{
    public class LogInsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public LogInsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: LogIns
        public async Task<IActionResult> Index()
        {
            return View(await _context.LogIn.ToListAsync());
        }

        // GET: LogIns/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var logIn = await _context.LogIn
                .FirstOrDefaultAsync(m => m.Id == id);
            if (logIn == null)
            {
                return NotFound();
            }

            return View(logIn);
        }

        // GET: LogIns/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: LogIns/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name")] LogIn logIn)
        {
            if (ModelState.IsValid)
            {
                _context.Add(logIn);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(logIn);
        }

        // GET: LogIns/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var logIn = await _context.LogIn.FindAsync(id);
            if (logIn == null)
            {
                return NotFound();
            }
            return View(logIn);
        }

        // POST: LogIns/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] LogIn logIn)
        {
            if (id != logIn.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(logIn);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LogInExists(logIn.Id))
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
            return View(logIn);
        }

        // GET: LogIns/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var logIn = await _context.LogIn
                .FirstOrDefaultAsync(m => m.Id == id);
            if (logIn == null)
            {
                return NotFound();
            }

            return View(logIn);
        }

        // POST: LogIns/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var logIn = await _context.LogIn.FindAsync(id);
            if (logIn != null)
            {
                _context.LogIn.Remove(logIn);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LogInExists(int id)
        {
            return _context.LogIn.Any(e => e.Id == id);
        }
    }
}
