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
        public async Task<IActionResult> Crear(IFormCollection form)
        {
            try
            {
                // Recibir datos del formulario
                string nombre = form["nombre"];
                string descripcion = form["descripcion"];
                decimal precio = Convert.ToDecimal(form["precio"]);
                string imagenBase64 = form["imagen"];

                // Crear una nueva instancia de Servicio
                var servicio = new Servicio
                {
                    Nombre = nombre,
                    Descripcion = descripcion,
                    Precio = precio
                };

                // Guardar la imagen en la carpeta images/servicios
                if (!string.IsNullOrEmpty(imagenBase64))
                {
                    // Limpiar la cadena base64 para eliminar el encabezado de los datos de imagen
                    string imageData = imagenBase64.Split(',').Last();
                    byte[] imageBytes = Convert.FromBase64String(imageData);

                    // Generar un nombre de archivo basado en el nombre del servicio
                    string sanitizedNombre = nombre.Replace(" ", "_").Replace("/", "_"); // Reemplazar espacios y barras por guiones bajos
                    string imageName = sanitizedNombre + ".jpg";
                    string imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/servicios", imageName);

                    // Guardar la imagen en el sistema de archivos
                    await System.IO.File.WriteAllBytesAsync(imagePath, imageBytes);

                    // Guardar la ruta de la imagen en la base de datos
                    servicio.Imagen = imagePath;
                }

                // Guardar el servicio en la base de datos
                _context.Servicios.Add(servicio);
                await _context.SaveChangesAsync();

                return Ok(); // Devuelve una respuesta exitosa
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        public async Task<IActionResult> Editar(IFormCollection form)
        {
            try
            {
                // Recibir los datos del formulario
                int servicioId = Convert.ToInt32(form["servicioId"]);
                string nombre = form["nombre"];
                string descripcion = form["descripcion"];
                decimal precio = Convert.ToDecimal(form["precio"]);
                string imagenBase64 = form["imagen"];

                // Obtener el servicio existente
                var servicio = await _context.Servicios.FindAsync(servicioId);
                if (servicio == null) return NotFound();

                // Actualizar los campos del servicio
                servicio.Nombre = nombre;
                servicio.Descripcion = descripcion;
                servicio.Precio = precio;

                // Si se ha recibido una imagen nueva, se reemplaza la imagen anterior
                if (!string.IsNullOrEmpty(imagenBase64))
                {
                    // Limpiar la cadena base64 para eliminar el encabezado de los datos de imagen
                    string imageData = imagenBase64.Split(',').Last();
                    byte[] imageBytes = Convert.FromBase64String(imageData);

                    // Generar un nombre de archivo basado en el nombre del servicio
                    string sanitizedNombre = nombre.Replace(" ", "_").Replace("/", "_"); // Reemplazar espacios y barras por guiones bajos
                    string imageName = sanitizedNombre + ".jpg";
                    string imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/servicios", imageName);

                    // Guardar la imagen en el sistema de archivos
                    await System.IO.File.WriteAllBytesAsync(imagePath, imageBytes);

                    // Guardar la ruta de la imagen en la base de datos
                    servicio.Imagen = imagePath;
                }

                _context.Servicios.Update(servicio);
                await _context.SaveChangesAsync();

                return Ok(); // Respuesta exitosa
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
            _context.Servicios.Remove(servicio);
            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}

