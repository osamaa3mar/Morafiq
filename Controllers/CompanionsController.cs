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
    public class CompanionsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CompanionsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Companions
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Companions.Include(p => p.Service);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Companions/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Companions == null)
            {
                return NotFound();
            }

            var Companion = await _context.Companions
                .Include(p => p.Service)
                .FirstOrDefaultAsync(m => m.CompanionId == id);
            if (Companion == null)
            {
                return NotFound();
            }

            return View(Companion);
        }

        // GET: Companions/Create
        public IActionResult Create()
        {
            ViewData["ServiceId"] = new SelectList(_context.Services, "ServiceId", "ServiceName");
            return View();
        }

        // POST: Companions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Companion Companion, IFormFile FormFile)
        {
            if (FormFile != null && FormFile.Length > 0)
            {

                using (var stream = FormFile.OpenReadStream())
                using (var reader = new BinaryReader(stream))
                {
                    var byteFile = reader.ReadBytes((int)stream.Length);
                    Companion.Image = byteFile;
                }

                Companion.ImageName = FormFile.FileName;
                Companion.contentType = FormFile.ContentType;


                _context.Add(Companion);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));

            }
            else
            {

                ModelState.AddModelError("Image", "Please select an image.");
            }



            return RedirectToAction("Companions","Admin",new { ServiceId  = Companion.ServiceId});
        }


        // GET: Companions/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Companions == null)
            {
                return NotFound();
            }

            var Companion = await _context.Companions.FindAsync(id);
            if (Companion == null)
            {
                return NotFound();
            }
            ViewData["ServiceId"] = new SelectList(_context.Services, "ServiceId", "ServiceName", Companion.ServiceId);
            return View(Companion);
        }

        // POST: Companions/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Companion Companion, IFormFile FormFile)
        {
            //if (id != Companion.CompanionId)
            //{
            //    return NotFound();
            //}

            //if (ModelState.IsValid)
            //{
            try
            {
                Companion p = _context.Companions.Where(p => p.CompanionId == Companion.CompanionId).SingleOrDefault();
                if (FormFile != null)
                {
                    using (var stream = FormFile.OpenReadStream())
                    using (var reader = new BinaryReader(stream))
                    {
                        var byteFile = reader.ReadBytes((int)stream.Length);
                        p.Image = byteFile;
                    }
                    p.ImageName = FormFile.FileName;
                    p.contentType = FormFile.ContentType;
                }
                p.CompanionPrice = Companion.CompanionPrice;
                p.CompanionSale = Companion.CompanionSale;
                p.CompanionDescription = Companion.CompanionDescription;
                p.CompanionName = Companion.CompanionName;
                p.CompanionQuantityStock = Companion.CompanionQuantityStock;
                p.ServiceId = Companion.ServiceId;
                _context.Update(p);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CompanionExists(Companion.CompanionId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction(nameof(Index));
            //}
            //ViewData["ServiceId"] = new SelectList(_context.Services, "ServiceId", "ServiceName", Companion.ServiceId);
            //return View(Companion);
        }

        // GET: Companions/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Companions == null)
            {
                return NotFound();
            }

            var Companion = await _context.Companions
                .Include(p => p.Service)
                .FirstOrDefaultAsync(m => m.CompanionId == id);
            if (Companion == null)
            {
                return NotFound();
            }

            return View(Companion);
        }

        // POST: Companions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Companions == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Companions'  is null.");
            }
            var Companion = await _context.Companions.FindAsync(id);
            if (Companion != null)
            {
                _context.Companions.Remove(Companion);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CompanionExists(int id)
        {
            return (_context.Companions?.Any(e => e.CompanionId == id)).GetValueOrDefault();
        }
    }
}
