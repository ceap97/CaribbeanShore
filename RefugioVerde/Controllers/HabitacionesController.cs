using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using RefugioVerde.Models;

namespace RefugioVerde.Controllers
{
    public class HabitacionesController : Controller
    {
        private readonly RefugioVerdeContext _context;
        private readonly string _uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "habitaciones");

        public HabitacionesController(RefugioVerdeContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Catalogo()
        {
            var habitaciones = await _context.Habitacions.Include(h => h.EstadoHabitacion).ToListAsync();
            return View(habitaciones);
        }

        // GET: /Habitaciones/Index
        public async Task<IActionResult> Index()
        {
            var habitaciones = await _context.Habitacions.Include(h => h.EstadoHabitacion).ToListAsync();
            return View(habitaciones);
        }

        // GET: /Habitaciones/Listar
        [HttpGet]
        public async Task<IActionResult> Listar()
        {
            var habitaciones = await _context.Habitacions.Include(h => h.EstadoHabitacion).ToListAsync();
            return Json(habitaciones);
        }

        // GET: /Habitaciones/Obtener/{id}
        [HttpGet]
        public async Task<IActionResult> Obtener(int id)
        {
            var habitacion = await _context.Habitacions.Include(h => h.EstadoHabitacion).FirstOrDefaultAsync(h => h.HabitacionId == id);
            if (habitacion == null)
            {
                return NotFound();
            }
            return Json(habitacion);
        }

        // POST: /Habitaciones/Crear
        [HttpPost]
        public async Task<IActionResult> Crear([FromForm] Habitacion habitacion, [FromForm] IFormFile imagen)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    // Si se proporciona una imagen, se guarda en el servidor
                    if (imagen != null && imagen.Length > 0)
                    {
                        var fileName = $"{habitacion.NombreHabitacion}.jpeg";  // Usar el ID de la habitación como nombre del archivo
                        var filePath = Path.Combine(_uploadPath, fileName);

                        // Crear el directorio si no existe
                        if (!Directory.Exists(_uploadPath))
                        {
                            Directory.CreateDirectory(_uploadPath);
                        }

                        // Guardar la imagen en el servidor
                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await imagen.CopyToAsync(stream);
                        }

                        habitacion.Imagen = $"/images/habitaciones/{fileName}";  // Guardar la ruta relativa en la base de datos
                    }

                    _context.Habitacions.Add(habitacion);
                    await _context.SaveChangesAsync();
                    return Ok();
                }
                return BadRequest(ModelState);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // POST: /Habitaciones/Editar
        [HttpPost]
        public async Task<IActionResult> Editar([FromForm] Habitacion habitacion, [FromForm] IFormFile imagen)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    // Si se proporciona una nueva imagen, se guarda en el servidor
                    if (imagen != null && imagen.Length > 0)
                    {
                        var fileName = $"{habitacion.NombreHabitacion}.jpeg";  // Usar el ID de la habitación como nombre del archivo
                        var filePath = Path.Combine(_uploadPath, fileName);

                        // Crear el directorio si no existe
                        if (!Directory.Exists(_uploadPath))
                        {
                            Directory.CreateDirectory(_uploadPath);
                        }

                        // Guardar la nueva imagen
                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await imagen.CopyToAsync(stream);
                        }

                        habitacion.Imagen = $"/images/habitaciones/{fileName}";  // Guardar la ruta relativa en la base de datos
                    }
                    else
                    {
                        // Si no se proporciona una nueva imagen, mantener la imagen existente
                        var existingHabitacion = await _context.Habitacions.AsNoTracking().FirstOrDefaultAsync(h => h.HabitacionId == habitacion.HabitacionId);
                        if (existingHabitacion != null)
                        {
                            habitacion.Imagen = existingHabitacion.Imagen;
                        }
                    }

                    _context.Habitacions.Update(habitacion);
                    await _context.SaveChangesAsync();
                    return Ok();
                }
                return BadRequest(ModelState);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // DELETE: /Habitaciones/Eliminar/{id}
        [HttpDelete]
        public async Task<IActionResult> Eliminar(int id)
        {
            var habitacion = await _context.Habitacions.FindAsync(id);
            if (habitacion == null)
            {
                return NotFound();
            }

            // Eliminar la imagen del servidor si existe
            if (!string.IsNullOrEmpty(habitacion.Imagen))
            {
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", habitacion.Imagen.TrimStart('/'));
                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);  // Eliminar archivo
                }
            }

            _context.Habitacions.Remove(habitacion);
            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}
