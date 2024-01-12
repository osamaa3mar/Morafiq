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
using SQLitePCL;
using Microsoft.AspNetCore.Http;

namespace _Morafiq.Controllers
{
	public class CartCompanionsController : Controller
	{
		private readonly ApplicationDbContext _context;

		public CartCompanionsController(ApplicationDbContext context)
		{
			_context = context;
		}

		// GET: CartCompanions
		public async Task<IActionResult> Index()
		{
			var applicationDbContext = _context.CartCompanions.Include(c => c.Companion);
			return View(await applicationDbContext.ToListAsync());
		}

		// GET: CartCompanions/Details/5
		public async Task<IActionResult> Details(int? id)
		{
			if (id == null || _context.CartCompanions == null)
			{
				return NotFound();
			}

			var cartCompanion = await _context.CartCompanions
				.Include(c => c.Companion)
				.FirstOrDefaultAsync(m => m.CartId == id);
			if (cartCompanion == null)
			{
				return NotFound();
			}

			return View(cartCompanion);
		}

		// GET: CartCompanions/Create
		public IActionResult Create()
		{
			ViewData["CompanionId"] = new SelectList(_context.Companions, "CompanionId", "CompanionDescription");
			return View();
		}

		// POST: CartCompanions/Create
		// To protect from overposting attacks, enable the specific properties you want to bind to.
		// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create_(int id, int quantity, string selectedStartDate, string selectedEndDate)
		{
			//if (ModelState.IsValid)
			//{
			var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
			if (userId == null)
			{
				var returnUrl = Url.Action("CompanionDetails", "Shop", new { id, quantity });

				return RedirectToPage("/Account/Login", new { area = "Identity", returnUrl });

			}

			//        new   work         //
			if (DateTime.TryParse(selectedStartDate, out DateTime parsedStartDate) &&
				DateTime.TryParse(selectedEndDate, out DateTime parsedEndDate))

			{

				CompanionSchedule newSchedule = new CompanionSchedule
				{
					CompanionId = id,
					UserId = userId,
					StartDate = parsedStartDate,
					EndDate = parsedEndDate,

					Status = "Pending"
				};
				_context.CompanionSchedule.Add(newSchedule);

				// Add the new schedule to the database


				await _context.SaveChangesAsync();





				//      end  new   work         //
				Companion Companion = _context.Companions.Where(Companion => Companion.CompanionId == id).SingleOrDefault();
				Cart cart = _context.Carts.Where(cart => cart.UserId == userId).SingleOrDefault();
				CartCompanion cartCompanion_ = _context.CartCompanions.Where(cartP => cartP.CartId == cart.CartId && cartP.CompanionId == id).SingleOrDefault();
				if (cartCompanion_ == null)
				{
					CartCompanion cartCompanion = new CartCompanion();
					cartCompanion.CompanionId = id;
					cartCompanion.StartDate = parsedStartDate;
					cartCompanion.EndDate = parsedEndDate;
					cartCompanion.CartId = cart.CartId;
					_context.Add(cartCompanion);
					await _context.SaveChangesAsync();
					if (Companion.CompanionSale > 0)
					{
						cart.TotalPrice = @Companion.CompanionPrice - (@Companion.CompanionPrice * @Companion.CompanionSale / 100);
					}
					else
					{
						cart.TotalPrice = Companion.CompanionPrice;
					}
					cart.TotalQuantity++;
					_context.Update(cart);
					await _context.SaveChangesAsync();
				}
				else
				{
					cartCompanion_.StartDate = parsedStartDate;
					cartCompanion_.EndDate = parsedEndDate;
					_context.Update(cartCompanion_);
					await _context.SaveChangesAsync();
					if (Companion.CompanionSale > 0)
					{
						cart.TotalPrice += @Companion.CompanionPrice - (@Companion.CompanionPrice * @Companion.CompanionSale / 100);
					}
					else
					{
						cart.TotalPrice +=  Companion.CompanionPrice;
					}
					_context.Update(cart);
					await _context.SaveChangesAsync();
				}
				_context.Update(Companion);
				await _context.SaveChangesAsync();
				return RedirectToAction("CompanionDetails", "Shop", new { id = id });
			}
			return RedirectToAction("CompanionDetails", "Shop", new { id = id });
			//}
			//ViewData["CompanionId"] = new SelectList(_context.Companions, "CompanionId", "CompanionDescription", cartCompanion.CompanionId);
			//return View(cartCompanion);
		}

		// GET: CartCompanions/Edit/5
		public async Task<IActionResult> Edit(int? id)
		{
			if (id == null || _context.CartCompanions == null)
			{
				return NotFound();
			}

			var cartCompanion = await _context.CartCompanions.FindAsync(id);
			if (cartCompanion == null)
			{
				return NotFound();
			}
			ViewData["CompanionId"] = new SelectList(_context.Companions, "CompanionId", "CompanionDescription", cartCompanion.CompanionId);
			return View(cartCompanion);
		}

		// POST: CartCompanions/Edit/5
		// To protect from overposting attacks, enable the specific properties you want to bind to.
		// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(int id, [Bind("CartId,CompanionQuantity,CompanionId")] CartCompanion cartCompanion)
		{
			if (id != cartCompanion.CartId)
			{
				return NotFound();
			}

			if (ModelState.IsValid)
			{
				try
				{
					_context.Update(cartCompanion);
					await _context.SaveChangesAsync();
				}
				catch (DbUpdateConcurrencyException)
				{
					if (!CartCompanionExists(cartCompanion.CartId))
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
			ViewData["CompanionId"] = new SelectList(_context.Companions, "CompanionId", "CompanionDescription", cartCompanion.CompanionId);
			return View(cartCompanion);
		}

		// GET: CartCompanions/Delete/5
		public async Task<IActionResult> Delete(int? id)
		{
			if (id == null || _context.CartCompanions == null)
			{
				return NotFound();
			}

			var cartCompanion = await _context.CartCompanions
				.Include(c => c.Companion)
				.FirstOrDefaultAsync(m => m.CartId == id);
			if (cartCompanion == null)
			{
				return NotFound();
			}

			return View(cartCompanion);
		}

		// POST: CartCompanions/Delete/5
		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> DeleteConfirmed(int id)
		{
			if (_context.CartCompanions == null)
			{
				return Problem("Entity set 'ApplicationDbContext.CartCompanions'  is null.");
			}
			var cartCompanion = await _context.CartCompanions.FindAsync(id);
			if (cartCompanion != null)
			{
				_context.CartCompanions.Remove(cartCompanion);
			}

			await _context.SaveChangesAsync();
			return RedirectToAction(nameof(Index));
		}

		private bool CartCompanionExists(int id)
		{
			return (_context.CartCompanions?.Any(e => e.CartId == id)).GetValueOrDefault();
		}
	}
}
