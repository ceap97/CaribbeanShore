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
        [HttpPost]
        public async Task<IActionResult> FiltrarHabitaciones(DateTime checkIn, DateTime checkOut, int adults, int children)
        {
            int totalCapacity = adults + children;
            var habitaciones = await _context.Habitacions
                .Include(h => h.EstadoHabitacion)
                .Where(h => h.EstadoHabitacion.Nombre == "Disponible" && h.Capacidad >= totalCapacity)
                .ToListAsync();

            var servicios = await _context.Servicios.ToListAsync();
            var comodidades = await _context.Comodidads.ToListAsync();
            var viewModel = new HomeViewModel
            {
                Habitaciones = habitaciones,
                Servicios = servicios,
                Comodidades = comodidades
            };

            return View("Cliente", viewModel);
        }

        public IActionResult Contact()
        {
            return View();
        }

        [HttpPost]
        public IActionResult SendContact(string name, string email, string message)
        {
            // Aquí puedes agregar la lógica para manejar el envío del formulario,
            // como enviar un correo electrónico o guardar el mensaje en una base de datos.

            // Por ahora, simplemente redirigimos al usuario a una página de confirmación.
            return RedirectToAction("ContactConfirmation");
        }

        public IActionResult ContactConfirmation()
        {
            return View();
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


        public async Task<IActionResult> Cliente()
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