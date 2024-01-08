using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using _Morafiq.Data;
using _Morafiq.Models;
using System.Security.Claims;

namespace _Morafiq.Controllers
{
    public class VisasController : Controller
    {
        private readonly ApplicationDbContext _context;

        public VisasController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Visas
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Visa.Include(v => v.User);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Visas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Visa == null)
            {
                return NotFound();
            }

            var visa = await _context.Visa
                .Include(v => v.User)
                .FirstOrDefaultAsync(m => m.VisaId == id);
            if (visa == null)
            {
                return NotFound();
            }

            return View(visa);
        }

        // GET: Visas/Create
        public IActionResult Create()
        {
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id");
            return View();
        }

        // POST: Visas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("VisaId,CVC,VisaNumber,ExpDate,UserId")] Visa visa)
        {
            //if (ModelState.IsValid)
            //{
            visa.UserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            _context.Add(visa);
                await _context.SaveChangesAsync();
                return RedirectToAction("Profile","Users");
            //}
            //ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", visa.UserId);
            //return View(visa);
        }

        // GET: Visas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Visa == null)
            {
                return NotFound();
            }

            var visa = await _context.Visa.FindAsync(id);
            if (visa == null)
            {
                return NotFound();
            }
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", visa.UserId);
            return View(visa);
        }

        // POST: Visas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("VisaId,CVC,VisaNumber,ExpDate,UserId")] Visa visa)
        {
            if (id != visa.VisaId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(visa);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VisaExists(visa.VisaId))
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
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", visa.UserId);
            return View(visa);
        }

        // GET: Visas/Delete/5
        public async Task<IActionResult> Delete(string? Id)
        {
            if (Id == null || _context.Visa == null)
            {
                return NotFound();
            }

            var visa = await _context.Visa
                .Include(v => v.User)
                .FirstOrDefaultAsync(m => m.UserId == Id);
            if (visa == null)
            {
                return NotFound();
            }

            return View(visa);
        }

        // POST: Visas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int VisaId)
        {
            if (_context.Visa == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Visa'  is null.");
            }
            var visa = await _context.Visa.FindAsync(VisaId);
            if (visa != null)
            {
                _context.Visa.Remove(visa);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction("Profile","Users");
        }

        private bool VisaExists(int id)
        {
          return (_context.Visa?.Any(e => e.VisaId == id)).GetValueOrDefault();
        }
    }
}
