using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using RefugioVerde.Models;
using System.Linq;
using System.Threading.Tasks;

namespace RefugioVerde.Controllers
{
    public class EncuestaSatisfaccionController : Controller
    {
        private readonly RefugioVerdeContext _context;

        public EncuestaSatisfaccionController(RefugioVerdeContext context)
        {
            _context = context;
        }

        // GET: EncuestaSatisfaccion/Create
        public IActionResult Create()
        {
            ViewBag.Habitaciones = new SelectList(_context.Habitacions, "HabitacionId", "NombreHabitacion");
            ViewBag.Empleados = new SelectList(_context.Empleados, "EmpleadoId", "Nombre");
            return View();
        }

        // POST: EncuestaSatisfaccion/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CalificacionGeneral,CalificacionHabitacion,CalificacionAtencion,Comentarios,HabitacionId,EmpleadoId")] EncuestaSatisfaccion encuestaSatisfaccion)
        {
            if (ModelState.IsValid)
            {
                _context.Add(encuestaSatisfaccion);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Gracias));
            }
            ViewBag.Habitaciones = new SelectList(_context.Habitacions, "HabitacionId", "NombreHabitacion", encuestaSatisfaccion.HabitacionId);
            ViewBag.Empleados = new SelectList(_context.Empleados, "EmpleadoId", "Nombre", encuestaSatisfaccion.EmpleadoId);
            return View(encuestaSatisfaccion);
        }

        // GET: EncuestaSatisfaccion/Gracias
        public IActionResult Gracias()
        {
            return View();
        }
    }
}
