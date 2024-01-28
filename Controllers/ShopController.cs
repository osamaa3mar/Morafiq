using _Morafiq.Data;
using _Morafiq.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using MimeKit;
using System.Net.Mail;
using System.Security.Claims;
using MailKit.Net.Smtp;
namespace _Morafiq.Controllers
{
    public class ShopController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;
        private readonly RoleManager<IdentityRole> _roleManager;
        public ShopController(ApplicationDbContext context, RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _roleManager = roleManager;
        }



        public IActionResult Index(string selectedService)
        {

            
            ViewBag.Companions = _context.Companions.Include(review => review.Reviews).Where(c => c.CompanionStatus == "Accept").ToList();
            ViewBag.Services = _context.Services.ToList();
            ViewBag.SelectedService = selectedService;
            //ViewBag.CompanionImages = _context.CompanionImages.ToList();


            return View();
        }
        public async Task<IActionResult> Service(int? id, string selectedService)
        {

            ViewBag.Companions = _context.Companions.Include(review => review.Reviews).Where(c => c.CompanionStatus == "Accept").ToList();
            ViewBag.Services = _context.Services.ToList();
            ViewBag.Reviews = _context.Reviews.Include(r => r.Companion).ThenInclude(c =>c.Service).Where(s =>s.Companion.ServiceId==id).ToList();
            ViewBag.SelectedService = selectedService;
            //ViewBag.CompanionImages = _context.CompanionImages.ToList();
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
        public async Task<IActionResult> CompanionDetails(int? id)
        {

            ViewBag.Companions = _context.Companions.Where(c => c.CompanionStatus == "Accept").ToList();
            ViewBag.Services = _context.Services.ToList();
            ViewBag.Reviews = _context.Reviews.Include(review => review.Companion).Include(review => review.User).Where(review => review.CompanionId == id && review.ReviewStatus == "Accept").ToList();
            //ViewBag.CompanionImages = _context.CompanionImages
            //.Where(pi => pi.CompanionId == id)
            //.ToList();

            if (id == null || _context.Services == null)
            {
                return NotFound();
            }

            var Companion = await _context.Companions
                .FirstOrDefaultAsync(m => m.CompanionId == id);
            if (Companion == null)
            {
                return NotFound();
            }
			ViewBag.UserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            ViewBag.CompanionSchedules = _context.CompanionSchedule.Where(s => s.CompanionId == id && s.Status=="Accept").ToList();
            return View(Companion);
        }
        public IActionResult UserCart()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            Cart cart = _context.Carts.Include(cartP => cartP.User).Where(cart => cart.UserId == userId).SingleOrDefault();
            ViewBag.Cart = cart;
            ViewBag.CartCompanions = _context.CartCompanions.Include(cartP => cartP.Companion).ThenInclude(Companion => Companion.Service).Where(cartP => cartP.CartId == cart.CartId).ToList();
            return View();

        }
        public async Task<IActionResult> RemoveCompanionFromCart(int CompanionId,int CartId)
        {
            Companion Companion = _context.Companions.Where(Companion => Companion.CompanionId == CompanionId).SingleOrDefault();
            CartCompanion cartCompanion = _context.CartCompanions.Include(cartP=>cartP.Companion).Where(cartP => cartP.CompanionId == CompanionId && cartP.CartId == CartId).SingleOrDefault();
            Cart cart = _context.Carts.Where(cart => cart.CartId == CartId).SingleOrDefault();
            cart.TotalQuantity--;
            if (Companion.CompanionSale > 0)
            {
                cart.TotalPrice -=  (Companion.CompanionPrice - (Companion.CompanionPrice * Companion.CompanionSale / 100));

            }
            else
            {
                cart.TotalPrice -=  Companion.CompanionPrice;
            }
            _context.Remove(cartCompanion);
            await _context.SaveChangesAsync();
            _context.Update(Companion);
            await _context.SaveChangesAsync();
            _context.Update(cart);
            await _context.SaveChangesAsync();
            return RedirectToAction("UserCart");
        }

        public async Task<IActionResult> RemoveOneItemFromCompanion(int CompanionId, int CartId)
        {
            Companion Companion = _context.Companions.Where(Companion => Companion.CompanionId == CompanionId).SingleOrDefault();
            CartCompanion cartCompanion = _context.CartCompanions.Include(cartP => cartP.Companion).Where(cartP => cartP.CompanionId == CompanionId && cartP.CartId == CartId).SingleOrDefault();
            Cart cart = _context.Carts.Where(cart => cart.CartId == CartId).SingleOrDefault();

            
                if (Companion.CompanionSale > 0)
                {
                    cart.TotalPrice -= (@Companion.CompanionPrice - (@Companion.CompanionPrice * @Companion.CompanionSale / 100));
                }
                else
                {
                    cart.TotalPrice -= Companion.CompanionPrice;
                }
                _context.Update(cartCompanion);
                await _context.SaveChangesAsync();
                _context.Update(cart);
                await _context.SaveChangesAsync();
                _context.Update(Companion);
                await _context.SaveChangesAsync();
            
            
            return RedirectToAction("UserCart");
        }
        public async Task<IActionResult> AddOneItemToCompanion(int CompanionId, int CartId)
        {
            Companion Companion = _context.Companions.Where(Companion => Companion.CompanionId == CompanionId).SingleOrDefault();
            CartCompanion cartCompanion = _context.CartCompanions.Include(cartP => cartP.Companion).Where(cartP => cartP.CompanionId == CompanionId && cartP.CartId == CartId).SingleOrDefault();
            Cart cart = _context.Carts.Where(cart => cart.CartId == CartId).SingleOrDefault();
            if (Companion.CompanionSale > 0)
            {
                cart.TotalPrice += (@Companion.CompanionPrice - (@Companion.CompanionPrice * @Companion.CompanionSale / 100));
            }
            else
            {
                cart.TotalPrice += Companion.CompanionPrice;
            }
            _context.Update(cartCompanion);
            await _context.SaveChangesAsync();
            _context.Update(cart);
            await _context.SaveChangesAsync();
            _context.Update(Companion);
            await _context.SaveChangesAsync();
            return RedirectToAction("UserCart");
        }
        public async Task<IActionResult> Checkout(decimal TotalPrice)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            Cart cart = _context.Carts.Include(cartP => cartP.User).Where(cart => cart.UserId == userId).SingleOrDefault();
            cart.TotalPrice = TotalPrice;
			_context.Update(cart);
			await _context.SaveChangesAsync();

			ViewBag.Cart = cart;
            ViewBag.CartCompanions = _context.CartCompanions.Where(cartP => cartP.CartId == cart.CartId).ToList();
            ViewBag.Visa = _context.Visa.Where(visa => visa.UserId == userId).SingleOrDefault();
            User user = _context.Users.Where(user=>user.Id == userId).SingleOrDefault();
            return View(user);
        }
        public async Task<IActionResult> Proceed()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            Cart cart = _context.Carts.Where(cart => cart.UserId == userId).SingleOrDefault();
            List<CartCompanion> cartCompanions = _context.CartCompanions.Where(cartP => cartP.CartId == cart.CartId).ToList();
            
		
            Order order = new Order();
            order.TotalPrice = cart.TotalPrice; 
            order.UserId = userId;
            order.Status = "Pending";
            order.OrderRate = 0;
            _context.Add(order);
            await _context.SaveChangesAsync();
            foreach (var cartCompanion in cartCompanions)
            {
                OrderCompanion orderCompanion = new OrderCompanion();
                orderCompanion.CompanionId = cartCompanion.CompanionId;
                orderCompanion.OrderId= order.OrderId;
                orderCompanion.CompanionQuantity = 1;
                orderCompanion.Companion =  _context.Companions.Include(c=>c.User).Where(c => c.CompanionId == cartCompanion.CompanionId).SingleOrDefault();
                _context.Add(orderCompanion);
                await _context.SaveChangesAsync();
                _context.Remove(cartCompanion);
                await _context.SaveChangesAsync();
				await SendOrderNoticeEmailToCompanion(orderCompanion);

			}
			//cart.TotalPrice += order;
			cart.TotalQuantity = 0;
			_context.Update(cart);
			await _context.SaveChangesAsync();
            
			return RedirectToAction("Index","Home");
        }
		public async Task<IActionResult> PayAfterApproveFromCompanion(int orderId)
        {
			var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            Cart cart = _context.Carts.Where(cart => cart.UserId == userId).SingleOrDefault();

            Order order1 = _context.Orders.Where(o => o.OrderId == orderId && o.IsPayed == false).FirstOrDefault();
			if (order1 != null)
			{
				if (order1.Status == "Accept")
				{
					Payment payment = new Payment();
					payment.OrderId = order1.OrderId;
					payment.Amount = order1.TotalPrice;
					payment.TransactionDate = DateTime.Now;
					_context.Add(payment);
                    order1.IsPayed = true;
                    _context.Update(order1);

                    cart.TotalPrice -= payment.Amount;
                    _context.Update(cart);
                    await _context.SaveChangesAsync();

					await SendOrderConfirmationEmail(order1);
 
                    return RedirectToAction("Index", "Orders", new { userId });
				}
                cart.TotalPrice -= order1.TotalPrice;
                _context.Update(cart);
                await _context.SaveChangesAsync();
            }
			return RedirectToAction("Index", "Home");
		}

		public IActionResult AddVisa()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateVisa(Visa visa)
        {
            visa.UserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            _context.Add(visa);
            await _context.SaveChangesAsync();
            return RedirectToAction("Checkout", "Shop");
        }
        public async Task SendOrderConfirmationEmail(Order order)
        {
            string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            User user = _context.Users.FirstOrDefault(u => u.Id == userId);

            var email = new MimeMessage();
            email.From.Add(new MailboxAddress("morafiq", "info.morafiq@gmail.com"));
            email.To.Add(new MailboxAddress($"{user.FirstName} {user.LastName}", user.Email));
            email.Subject = "Order Confirmation";

            var bodyBuilder = new BodyBuilder();
            bodyBuilder.TextBody = $"Dear {user.FirstName} {user.LastName},\n\n"
                             + "Thank you for choosing us!\n"
                             + $"Your order (Order ID: {order.OrderId}) has been successfully placed.\n"
                             + $"Order Total: {order.TotalPrice} JD\n"
                             + $"Companion will reach you soon\n"
                             + "We appreciate your business and look forward to serving you again!\n\n"
                             + "Best regards,\n"
                             + "Morafiq Team";

            email.Body = bodyBuilder.ToMessageBody();

            using (var smtp = new MailKit.Net.Smtp.SmtpClient())
            {
                smtp.ServerCertificateValidationCallback = (sender, certificate, chain, errors) => true;
				smtp.Connect("smtp.gmail.com", 587, false);
                smtp.Authenticate("info.morafiq@gmail.com", "tvup zspi qqxm akxb\r\n");
                await smtp.SendAsync(email);
                smtp.Disconnect(true);
            }
        }
		public async Task SendOrderNoticeEmailToCompanion(OrderCompanion order)
		{
			string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
			User user = _context.Users.FirstOrDefault(u => u.Id == userId);
            
			var email = new MimeMessage();
			email.From.Add(new MailboxAddress("morafiq", "info.morafiq@gmail.com"));
			email.To.Add(new MailboxAddress($"{order.Companion.User.FirstName} {order.Companion.User.LastName}", order.Companion.User.Email));
			email.Subject = "New Order";

			var bodyBuilder = new BodyBuilder();
			bodyBuilder.TextBody = $"Dear {order.Companion.User.FirstName} {order.Companion.User.LastName},\n\n"
							 + $"A new order (Order ID: {order.OrderId}) has been placed to you.\n"
							 + $"Name: {user.FirstName} {user.LastName}\n"
							 + $"Email: {user.Email}\n"
							 + $"Please Contact {user.FirstName} for more details and visit the website to approve\n"
							 + "We appreciate your business and look forward to serving you again!\n\n"
							 + "Best regards,\n"
							 + "Morafiq Team";

			email.Body = bodyBuilder.ToMessageBody();

			using (var smtp = new MailKit.Net.Smtp.SmtpClient())
			{
				smtp.ServerCertificateValidationCallback = (sender, certificate, chain, errors) => true;
				smtp.Connect("smtp.gmail.com", 587, false);
				smtp.Authenticate("info.morafiq@gmail.com", "tvup zspi qqxm akxb\r\n");
				await smtp.SendAsync(email);
				smtp.Disconnect(true);
			}
		}
	}
}
