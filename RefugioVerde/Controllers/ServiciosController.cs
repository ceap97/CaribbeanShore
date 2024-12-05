using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RefugioVerde.Models;

namespace RefugioVerde.Controllers
{
    public class ServiciosController : Controller
    {
        private readonly RefugioVerdeContext _context;
        private readonly string _uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "servicios");

        public ServiciosController(RefugioVerdeContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> CatalogoServicios()
        {
            var servicios = await _context.Servicios.ToListAsync();
            return View("CatalogoServicios", servicios);  // Cambiamos la vista a "Catalogo"
        }

        public async Task<IActionResult> Index()
        {
            var servicios = await _context.Servicios.ToListAsync();
            return View(servicios);
        }

        [HttpGet]
        public async Task<IActionResult> Listar()
        {
            var servicios = await _context.Servicios.ToListAsync();
            return Json(servicios);
        }

        // GET: /Servicios/Obtener/{id}
        [HttpGet]
        public async Task<IActionResult> Obtener(int id)
        {
            var servicio = await _context.Servicios.FindAsync(id);
            if (servicio == null)
            {
                return NotFound();
            }
            return Json(servicio);
        }

        // POST: /Servicios/Crear
        [HttpPost]
        public async Task<IActionResult> Crear([FromForm] Servicio servicio, [FromForm] IFormFile imagen)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    // Si se proporciona una imagen, se guarda en el servidor
                    if (imagen != null && imagen.Length > 0)
                    {
                        var fileName = $"{servicio.Nombre.Replace(" ", "_")}.jpeg";  // Usar el nombre del servicio como nombre del archivo
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

                        servicio.Imagen = $"/images/servicios/{fileName}";  // Guardar la ruta relativa en la base de datos
                    }

                    _context.Servicios.Add(servicio);
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

        // POST: /Servicios/Editar
        [HttpPost]
        public async Task<IActionResult> Editar([FromForm] Servicio servicio, [FromForm] IFormFile imagen)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    // Si se proporciona una nueva imagen, se guarda en el servidor
                    if (imagen != null && imagen.Length > 0)
                    {
                        var fileName = $"{servicio.Nombre.Replace(" ", "_")}.jpeg";  // Usar el nombre del servicio como nombre del archivo
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

                        servicio.Imagen = $"/images/servicios/{fileName}";  // Guardar la ruta relativa en la base de datos
                    }
                    else
                    {
                        // Si no se proporciona una nueva imagen, mantener la imagen existente
                        var existingServicio = await _context.Servicios.AsNoTracking().FirstOrDefaultAsync(s => s.ServicioId == servicio.ServicioId);
                        if (existingServicio != null)
                        {
                            servicio.Imagen = existingServicio.Imagen;
                        }
                    }

                    _context.Servicios.Update(servicio);
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

        // DELETE: /Servicios/Eliminar/{id}
        [HttpDelete]
        public async Task<IActionResult> Eliminar(int id)
        {
            var servicio = await _context.Servicios.FindAsync(id);
            if (servicio == null)
            {
                return NotFound();
            }

            // Eliminar la imagen del servidor si existe
            if (!string.IsNullOrEmpty(servicio.Imagen))
            {
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", servicio.Imagen.TrimStart('/'));
                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);  // Eliminar archivo
                }
            }

            _context.Servicios.Remove(servicio);
            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}
