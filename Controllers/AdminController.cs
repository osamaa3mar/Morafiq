using _Morafiq.Data;
using _Morafiq.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace _Morafiq.Controllers
{
    [Authorize(Roles = "ADMIN")]
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;
        public AdminController(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            var Id = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var user = _context.Users.Where(user => user.Id == Id).SingleOrDefault();
            ViewBag.Users = _context.Users.Where(user => user.Role == "User").ToList();
            ViewBag.Orders = _context.Orders.ToList();
            ViewBag.Services = _context.Services.ToList(); 

		    ViewBag.CompanionSchedule = _context.CompanionSchedule.Include(s => s.Companion).ThenInclude(Companion => Companion.User).ToList();
			ViewBag.Companions = _context.Companions.Include(product => product.Service).ToList();
            ViewBag.Payments = _context.Payments.Include(payment => payment.Order).ThenInclude(order => order.User).ToList();
            return View(user);
        }
        public IActionResult Users()
        {
            var Id = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var user = _context.Users.Where(user => user.Id == Id).SingleOrDefault();
            ViewBag.Users = _context.Users.Where(user => user.Role == "User").ToList();
            return View(user);
        }
        public IActionResult Services()
        {
            var Id = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var user = _context.Users.Where(user => user.Id == Id).SingleOrDefault();
            ViewBag.Services = _context.Services.Include(Service => Service.Companions).ToList();
            return View(user);
        }
        public IActionResult Orders()
        {
            var Id = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var user = _context.Users.Where(user => user.Id == Id).SingleOrDefault();
            ViewBag.Orders = _context.Orders.Include(order => order.User).ToList();
            return View(user);
        }
        public IActionResult CompanionReviews()
        {
            var Id = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var user = _context.Users.Where(user => user.Id == Id).SingleOrDefault();
            ViewBag.Reviews = _context.Reviews.Include(review => review.Companion).Include(review => review.User).ToList();
            return View(user);
        }
        public IActionResult Testimonials()
        {
            var Id = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var user = _context.Users.Where(user => user.Id == Id).SingleOrDefault();
            ViewBag.Testimonials = _context.Testimonials.Include(testimonial => testimonial.User).ToList();
            return View(user);
        }
		public IActionResult Companions(int ServiceId)
		{
			var Id = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
			var user = _context.Users.Where(user => user.Id == Id).SingleOrDefault();
			ViewBag.Companions = _context.Companions.Include(product => product.Service).Where(product => product.ServiceId == ServiceId).ToList();
            return View(user);
        }
    }
}
