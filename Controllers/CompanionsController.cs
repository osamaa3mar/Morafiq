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
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

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
		[Authorize(Roles = "COMPANION")]
		public IActionResult Index()
        {
            var Id = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var user = _context.Users.Where(user => user.Id == Id).SingleOrDefault();
            ViewBag.Services = _context.Services.ToList();
            ViewBag.Companions = _context.Companions.Include(product => product.Service).ToList();
            ViewBag.Payments = _context.Payments.Include(payment => payment.Order).ThenInclude(order => order.User).ToList();
            return View(user);
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
            ViewBag.ServiceId = new SelectList(_context.Services, "ServiceId", "ServiceName");
            ViewBag.UserId = new SelectList(_context.Users.Where(n => n.Role != "ADMIN" ), "Id", "Email");
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
                Companion.User = _context.Users.Where(n => n.Id == Companion.UserId).FirstOrDefault();
                Companion.CompanionName = Companion.User.FirstName + " " + Companion.User.LastName;
                Companion.User.Role = "COMPANION";
                var userRole =  _context.UserRoles.Where(c => c.UserId == Companion.UserId).FirstOrDefault();
                userRole.RoleId = "3";
                _context.Update(userRole);

                _context.Add(Companion);
                await _context.SaveChangesAsync();
                return RedirectToAction("Services","Admin");

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
			return RedirectToAction("Services", "Admin");
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
			return RedirectToAction("Services", "Admin");
		}

        private bool CompanionExists(int id)
        {
            return (_context.Companions?.Any(e => e.CompanionId == id)).GetValueOrDefault();
        }
    }
}
