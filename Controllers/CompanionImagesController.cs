using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using _5Dots.Data;
using _5Dots.Models;

namespace _5Dots.Controllers
{
    public class CompanionImagesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CompanionImagesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: CompanionImages
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.CompanionImages.Include(p => p.Companion);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: CompanionImages/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.CompanionImages == null)
            {
                return NotFound();
            }

            var CompanionImage = await _context.CompanionImages
                .Include(p => p.Companion)
                .FirstOrDefaultAsync(m => m.ImageId == id);
            if (CompanionImage == null)
            {
                return NotFound();
            }

            return View(CompanionImage);
        }

        // GET: CompanionImages/Create
        public IActionResult Create()
        {
            ViewData["CompanionId"] = new SelectList(_context.Companions, "CompanionId", "CompanionDescription");
            return View();
        }

        // POST: CompanionImages/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ImageId,CompanionId,ImageName,ContentType,Image")] CompanionImage CompanionImage,IFormFile FormFile)
        {
            //if (ModelState.IsValid)
            //{
                using (var stream = FormFile.OpenReadStream())
                using (var reader = new BinaryReader(stream))
                {
                    var byteFile = reader.ReadBytes((int)stream.Length);
                    CompanionImage.Image = byteFile;
                }
                CompanionImage.ImageName = FormFile.FileName;
                CompanionImage.ContentType = FormFile.ContentType;
                _context.Add(CompanionImage);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            //}
            //ViewData["CompanionId"] = new SelectList(_context.Companions, "CompanionId", "CompanionDescription", CompanionImage.CompanionId);
            //return View(CompanionImage);
        }

        // GET: CompanionImages/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.CompanionImages == null)
            {
                return NotFound();
            }

            var CompanionImage = await _context.CompanionImages.FindAsync(id);
            if (CompanionImage == null)
            {
                return NotFound();
            }
            ViewData["CompanionId"] = new SelectList(_context.Companions, "CompanionId", "CompanionDescription", CompanionImage.CompanionId);
            return View(CompanionImage);
        }

        // POST: CompanionImages/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id,CompanionImage CompanionImage,IFormFile FormFile)
        {
            if (id != CompanionImage.ImageId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    using (var stream = FormFile.OpenReadStream())
                    using (var reader = new BinaryReader(stream))
                    {
                        var byteFile = reader.ReadBytes((int)stream.Length);
                        CompanionImage.Image = byteFile;
                    }
                    CompanionImage.ImageName = FormFile.FileName;
                    CompanionImage.ContentType = FormFile.ContentType;
                    _context.Update(CompanionImage);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CompanionImageExists(CompanionImage.ImageId))
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
            ViewData["CompanionId"] = new SelectList(_context.Companions, "CompanionId", "CompanionDescription", CompanionImage.CompanionId);
            return View(CompanionImage);
        }

        // GET: CompanionImages/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.CompanionImages == null)
            {
                return NotFound();
            }

            var CompanionImage = await _context.CompanionImages
                .Include(p => p.Companion)
                .FirstOrDefaultAsync(m => m.ImageId == id);
            if (CompanionImage == null)
            {
                return NotFound();
            }

            return View(CompanionImage);
        }

        // POST: CompanionImages/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.CompanionImages == null)
            {
                return Problem("Entity set 'ApplicationDbContext.CompanionImages'  is null.");
            }
            var CompanionImage = await _context.CompanionImages.FindAsync(id);
            if (CompanionImage != null)
            {
                _context.CompanionImages.Remove(CompanionImage);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CompanionImageExists(int id)
        {
          return (_context.CompanionImages?.Any(e => e.ImageId == id)).GetValueOrDefault();
        }
    }
}
