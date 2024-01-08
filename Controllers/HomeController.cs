using _Morafiq.Data;
using _Morafiq.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Diagnostics;
using System.Security.Claims;

namespace _Morafiq.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;
        private readonly RoleManager<IdentityRole> _roleManager;
		private readonly UserManager<User> _userManager;
		public HomeController(ApplicationDbContext context,RoleManager<IdentityRole> roleManager,UserManager<User> userManager)
        {
            _context = context;
            _roleManager = roleManager;
            _userManager = userManager;
        }

     

        public async Task<IActionResult> IndexAsync(string selectedService)
        {
			var user = await _userManager.GetUserAsync(User);
           
			ViewBag.Companions = _context.Companions.Include(Companion => Companion.Service).ToList();
            ViewBag.Services = _context.Services.Include(Service => Service.Companions).ToList();
            ViewBag.SelectedService = selectedService;
            ViewBag.CompanionImages = _context.CompanionImages.ToList();
            ViewBag.Testimonials = _context.Testimonials.Include(testimonial => testimonial.User).Where(testimonial => testimonial.TestimonialStatus == "Accept").Take(3).ToList();
            return View(user);
        }




        public IActionResult Privacy()
        {
            return View();
        }
        public IActionResult Contact()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
