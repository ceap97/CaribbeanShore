using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RefugioVerde.Models;
using System.Diagnostics;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;

namespace RefugioVerde.Controllers
{
    public class HomeController : Controller
    {
        private readonly RefugioVerdeContext _context;
        private readonly ILogger<HomeController> _logger;

        public HomeController(RefugioVerdeContext context, ILogger<HomeController> logger)
        {
            _context = context;
            _logger = logger;
        }


        public async Task<IActionResult> Index()
        {
            var habitaciones = await _context.Habitacions.Include(h => h.EstadoHabitacion).ToListAsync();
            var servicios = await _context.Servicios.ToListAsync();
            var comodidades = await _context.Comodidads.ToListAsync();
            var viewModel = new HomeViewModel
            {
                Habitaciones = habitaciones,
                Servicios = servicios,
                Comodidades = comodidades
            };
            return View(viewModel);
        }


        public IActionResult Cliente()
        {
            ClaimsPrincipal claimuser = HttpContext.User;
            string nombreUsuario = "";

            if (claimuser.Identity.IsAuthenticated)
            {
                nombreUsuario = claimuser.Claims.Where(c => c.Type == ClaimTypes.Name)
                    .Select(c => c.Value).SingleOrDefault();
            }
            ViewData["nombreUsuario"] = nombreUsuario;
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        public async Task<IActionResult> CerrarSesion()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return RedirectToAction("Index", "Home");
        }
        [Authorize]
        public IActionResult Admin()
        {
            ClaimsPrincipal claimuser = HttpContext.User;
            string nombreUsuario = "";
            bool esEmpleado = false;

            if (claimuser.Identity.IsAuthenticated)
            {
                nombreUsuario = claimuser.Claims.Where(c => c.Type == ClaimTypes.Name)
                    .Select(c => c.Value).SingleOrDefault();
                esEmpleado = claimuser.Claims.Any(c => c.Type == "EmpleadoId" && !string.IsNullOrEmpty(c.Value));
            }

            if (!esEmpleado)
            {
                return RedirectToAction("Index", "Home");
            }

            ViewData["nombreUsuario"] = nombreUsuario;
            return View();
        }
    }
}