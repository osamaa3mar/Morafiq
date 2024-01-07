using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using _5Dots.Data;
using _5Dots.Models;
using System.Security.Claims;

namespace _5Dots.Controllers
{
    public class OrderCompanionsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public OrderCompanionsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: OrderCompanions
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.OrderCompanion.Include(o => o.Order).Include(o => o.Companion);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: OrderCompanions/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.OrderCompanion == null)
            {
                return NotFound();
            }

            var orderCompanion = await _context.OrderCompanion
                .Include(o => o.Order)
                .Include(o => o.Companion)
                .FirstOrDefaultAsync(m => m.OrderId == id);
            if (orderCompanion == null)
            {
                return NotFound();
            }

            return View(orderCompanion);
        }

        // GET: OrderCompanions/Create
        public IActionResult Create()
        {
            ViewData["OrderId"] = new SelectList(_context.Orders, "OrderId", "Status");
            ViewData["CompanionId"] = new SelectList(_context.Companions, "CompanionId", "CompanionDescription");
            return View();
        }

        // POST: OrderCompanions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("OrderId,CompanionId,Quantity")] OrderCompanion orderCompanion)
        {
            if (ModelState.IsValid)
            {
                _context.Add(orderCompanion);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["OrderId"] = new SelectList(_context.Orders, "OrderId", "Status", orderCompanion.OrderId);
            ViewData["CompanionId"] = new SelectList(_context.Companions, "CompanionId", "CompanionDescription", orderCompanion.CompanionId);
            return View(orderCompanion);
        }

        // GET: OrderCompanions/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.OrderCompanion == null)
            {
                return NotFound();
            }

            var orderCompanion = await _context.OrderCompanion.FindAsync(id);
            if (orderCompanion == null)
            {
                return NotFound();
            }
            ViewData["OrderId"] = new SelectList(_context.Orders, "OrderId", "Status", orderCompanion.OrderId);
            ViewData["CompanionId"] = new SelectList(_context.Companions, "CompanionId", "CompanionDescription", orderCompanion.CompanionId);
            return View(orderCompanion);
        }

        // POST: OrderCompanions/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("OrderId,CompanionId,Quantity")] OrderCompanion orderCompanion)
        {
            if (id != orderCompanion.OrderId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(orderCompanion);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrderCompanionExists(orderCompanion.OrderId))
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
            ViewData["OrderId"] = new SelectList(_context.Orders, "OrderId", "Status", orderCompanion.OrderId);
            ViewData["CompanionId"] = new SelectList(_context.Companions, "CompanionId", "CompanionDescription", orderCompanion.CompanionId);
            return View(orderCompanion);
        }

        // GET: OrderCompanions/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.OrderCompanion == null)
            {
                return NotFound();
            }

            var orderCompanion = await _context.OrderCompanion
                .Include(o => o.Order)
                .Include(o => o.Companion)
                .FirstOrDefaultAsync(m => m.OrderId == id);
            if (orderCompanion == null)
            {
                return NotFound();
            }

            return View(orderCompanion);
        }

        // POST: OrderCompanions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.OrderCompanion == null)
            {
                return Problem("Entity set 'ApplicationDbContext.OrderCompanion'  is null.");
            }
            var orderCompanion = await _context.OrderCompanion.FindAsync(id);
            if (orderCompanion != null)
            {
                _context.OrderCompanion.Remove(orderCompanion);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public IActionResult OrderItems(int orderId)
        {
            var Id = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var user = _context.Users.Where(user => user.Id == Id).SingleOrDefault();
            ViewBag.OrderCompanions = _context.OrderCompanion.Include(orderCompanion => orderCompanion.Companion).Include(orderCompanion => orderCompanion.Order).ThenInclude(order => order.User).Where(orderCompanion => orderCompanion.OrderId == orderId).ToList();
            ViewBag.TotalPrice = _context.Orders.Where(order => order.OrderId == orderId).SingleOrDefault().TotalPrice;
            return View(user);

        }
        private bool OrderCompanionExists(int id)
        {
          return (_context.OrderCompanion?.Any(e => e.OrderId == id)).GetValueOrDefault();
        }
    }
}
