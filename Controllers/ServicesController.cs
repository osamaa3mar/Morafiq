using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using _Morafiq.Data;
using _Morafiq.Models;
using Microsoft.AspNetCore.Authorization;

namespace _Morafiq.Controllers
{
    //[Authorize(Roles = "ADMIN")]
    public class ServicesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ServicesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Services
        public async Task<IActionResult> Index()
        {
              return _context.Services != null ? 
                          View(await _context.Services.ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.Services'  is null.");
        }

        // GET: Services/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Services == null)
            {
                return NotFound();
            }

            var Service = await _context.Services
                .FirstOrDefaultAsync(m => m.ServiceId == id);
            if (Service == null)
            {
                return NotFound();
            }

            return View(Service);
        }

        // GET: Services/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Services/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Service Service, IFormFile FormFile)
        {
            //if (ModelState.IsValid)
            //{
                using (var stream = FormFile.OpenReadStream())
                using (var reader = new BinaryReader(stream))
                {
                    var byteFile = reader.ReadBytes((int)stream.Length);
                    Service.Image = byteFile;
                }
                Service.ImageName = FormFile.FileName;
                Service.contentType = FormFile.ContentType;
                _context.Add(Service);
                await _context.SaveChangesAsync();
                return RedirectToAction("Services","Admin");
            //}
            //return View(Service);
        }

        // GET: Services/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Services == null)
            {
                return NotFound();
            }

            var Service = await _context.Services.FindAsync(id);
            if (Service == null)
            {
                return NotFound();
            }
            return View(Service);
        }

        // POST: Services/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Service Service,IFormFile FormFile)
        {
            if (id != Service.ServiceId)
            {
                return NotFound();
            }

            //if (ModelState.IsValid)
            //{
                try
                {
                    using (var stream = FormFile.OpenReadStream())
                    using (var reader = new BinaryReader(stream))
                    {
                        var byteFile = reader.ReadBytes((int)stream.Length);
                        Service.Image = byteFile;
                    }
                    Service.ImageName = FormFile.FileName;
                    Service.contentType = FormFile.ContentType;
                    _context.Update(Service);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ServiceExists(Service.ServiceId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Services", "Admin");
            //}
            //return View(Service);
        }

        // GET: Services/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Services == null)
            {
                return NotFound();
            }

            var Service = await _context.Services
                .FirstOrDefaultAsync(m => m.ServiceId == id);
            if (Service == null)
            {
                return NotFound();
            }

            return View(Service);
        }

        // POST: Services/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Services == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Services'  is null.");
            }
            var Service = await _context.Services.FindAsync(id);
            if (Service != null)
            {
                _context.Services.Remove(Service);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction("Services", "Admin");
        }

        private bool ServiceExists(int id)
        {
          return (_context.Services?.Any(e => e.ServiceId == id)).GetValueOrDefault();
        }
    }
}
