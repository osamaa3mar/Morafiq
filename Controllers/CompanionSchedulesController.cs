using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using _Morafiq.Data;
using _Morafiq.Models;

namespace _Morafiq.Controllers
{
    public class CompanionSchedulesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CompanionSchedulesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: CompanionSchedules
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.CompanionSchedule.Include(c => c.Companion);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: CompanionSchedules/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.CompanionSchedule == null)
            {
                return NotFound();
            }

            var companionSchedule = await _context.CompanionSchedule
                .Include(c => c.Companion)
                .FirstOrDefaultAsync(m => m.ScheduleId == id);
            if (companionSchedule == null)
            {
                return NotFound();
            }

            return View(companionSchedule);
        }

        // GET: CompanionSchedules/Create
        public IActionResult Create()
        {
            ViewData["CompanionId"] = new SelectList(_context.Companions, "CompanionId", "CompanionDescription");
            return View();
        }

        // POST: CompanionSchedules/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ScheduleId,CompanionId,ScheduleDate,Status")] CompanionSchedule companionSchedule)
        {
            //if (ModelState.IsValid)
            //{
                _context.Add(companionSchedule);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            //}
            //ViewData["CompanionId"] = new SelectList(_context.Companions, "CompanionId", "CompanionDescription", companionSchedule.CompanionId);
            //return View(companionSchedule);
        }

        // GET: CompanionSchedules/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.CompanionSchedule == null)
            {
                return NotFound();
            }

            var companionSchedule = await _context.CompanionSchedule.FindAsync(id);
            if (companionSchedule == null)
            {
                return NotFound();
            }
            ViewData["CompanionId"] = new SelectList(_context.Companions, "CompanionId", "CompanionDescription", companionSchedule.CompanionId);
            return View(companionSchedule);
        }

        // POST: CompanionSchedules/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ScheduleId,CompanionId,ScheduleDate,Status")] CompanionSchedule companionSchedule)
        {
            if (id != companionSchedule.ScheduleId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(companionSchedule);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CompanionScheduleExists(companionSchedule.ScheduleId))
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
            ViewData["CompanionId"] = new SelectList(_context.Companions, "CompanionId", "CompanionDescription", companionSchedule.CompanionId);
            return View(companionSchedule);
        }

        // GET: CompanionSchedules/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.CompanionSchedule == null)
            {
                return NotFound();
            }

            var companionSchedule = await _context.CompanionSchedule
                .Include(c => c.Companion)
                .FirstOrDefaultAsync(m => m.ScheduleId == id);
            if (companionSchedule == null)
            {
                return NotFound();
            }

            return View(companionSchedule);
        }

        // POST: CompanionSchedules/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.CompanionSchedule == null)
            {
                return Problem("Entity set 'ApplicationDbContext.CompanionSchedule'  is null.");
            }
            var companionSchedule = await _context.CompanionSchedule.FindAsync(id);
            if (companionSchedule != null)
            {
                _context.CompanionSchedule.Remove(companionSchedule);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CompanionScheduleExists(int id)
        {
          return (_context.CompanionSchedule?.Any(e => e.ScheduleId == id)).GetValueOrDefault();
        }
    }
}
