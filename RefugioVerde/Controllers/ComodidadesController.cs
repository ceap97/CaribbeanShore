using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RefugioVerde.Models;

namespace RefugioVerde.Controllers
{
    public class ComodidadesController : Controller
    {
        private readonly RefugioVerdeContext _context;

        public ComodidadesController(RefugioVerdeContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> CatalogoComodidad()
        {
            var comodidades = await _context.Comodidads.ToListAsync();
            return View(comodidades);
        }
        public async Task<IActionResult> Index()
        {
            var comodidades = await _context.Comodidads.ToListAsync();
            return View(comodidades);
        }

        [HttpGet]
        public async Task<IActionResult> Listar()
        {
            var comodidades = await _context.Comodidads.ToListAsync();
            return Json(comodidades);
        }

        // GET: /Comodidades/Obtener/{id}
        [HttpGet]
        public async Task<IActionResult> Obtener(int id)
        {
            var comodidad = await _context.Comodidads.FindAsync(id);
            if (comodidad == null)
            {
                return NotFound();
            }
            return Json(comodidad);
        }

        [HttpPost]
        public async Task<IActionResult> Crear(IFormCollection form)
        {
            // Recibir datos del formulario
            string nombre = form["nombre"];
            string descripcion = form["descripcion"];
            decimal precio = Convert.ToDecimal(form["precio"]);
            string imagenBase64 = form["imagen"];

            // Crear una nueva instancia de Comodidad
            var comodidad = new Comodidad
            {
                Nombre = nombre,
                Descripcion = descripcion,
                Precio = precio
            };

            // Guardar la imagen en la carpeta images/comodidades
            if (!string.IsNullOrEmpty(imagenBase64))
            {
                // Limpiar la cadena base64 para eliminar el encabezado de los datos de imagen
                string imageData = imagenBase64.Replace("data:image/jpeg;base64,", "");
                byte[] imageBytes = Convert.FromBase64String(imageData);

                // Generar un nombre de archivo basado en el nombre de la comodidad
                string sanitizedNombre = nombre.Replace(" ", "_").Replace("/", "_"); // Reemplazar espacios y barras por guiones bajos
                string imageName = sanitizedNombre + ".jpg";
                string imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/comodidades", imageName);

                // Guardar la imagen en el sistema de archivos
                await System.IO.File.WriteAllBytesAsync(imagePath, imageBytes);

                // Guardar la ruta de la imagen en la base de datos
                comodidad.Imagen = "/images/comodidades/" + imageName;
            }

            // Guardar la comodidad en la base de datos
            _context.Comodidads.Add(comodidad);
            await _context.SaveChangesAsync();

            return Ok(); // Devuelve una respuesta exitosa
        }



        // POST: /Comodidades/Editar
        [HttpPost]
        public async Task<IActionResult> Editar(IFormCollection form)
        {
            // Recibir los datos del formulario
            int comodidadId = Convert.ToInt32(form["comodidadId"]);
            string nombre = form["nombre"];
            string descripcion = form["descripcion"];
            decimal precio = Convert.ToDecimal(form["precio"]);
            string imagenBase64 = form["imagen"];

            // Obtener la comodidad existente
            var comodidad = await _context.Comodidads.FindAsync(comodidadId);
            if (comodidad == null) return NotFound();

            // Actualizar los campos de la comodidad
            comodidad.Nombre = nombre;
            comodidad.Descripcion = descripcion;
            comodidad.Precio = precio;

            // Si se ha recibido una imagen nueva, se reemplaza la imagen anterior
            if (!string.IsNullOrEmpty(imagenBase64))
            {
                // Limpiar la cadena base64 para eliminar el encabezado de los datos de imagen
                string imageData = imagenBase64.Replace("data:image/jpeg;base64,", "");
                byte[] imageBytes = Convert.FromBase64String(imageData);

                // Generar un nombre de archivo basado en el nombre de la comodidad
                string sanitizedNombre = nombre.Replace(" ", "_").Replace("/", "_"); // Reemplazar espacios y barras por guiones bajos
                string imageName = sanitizedNombre + ".jpg";
                string imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/comodidades", imageName);

                // Guardar la imagen en el sistema de archivos
                await System.IO.File.WriteAllBytesAsync(imagePath, imageBytes);

                // Guardar la ruta de la imagen en la base de datos
                comodidad.Imagen = "/images/comodidades/" + imageName;
            }

            _context.Comodidads.Update(comodidad);
            await _context.SaveChangesAsync();

            return Ok(); // Respuesta exitosa
        }



        // DELETE: /Comodidades/Eliminar/{id}
        [HttpDelete]
        public async Task<IActionResult> Eliminar(int id)
        {
            var comodidad = await _context.Comodidads.FindAsync(id);
            if (comodidad == null)
            {
                return NotFound();
            }
            _context.Comodidads.Remove(comodidad);
            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}
