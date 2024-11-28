using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RefugioVerde.Models;

namespace RefugioVerde.Controllers
{
    public class PagosController : Controller
    {
        private readonly RefugioVerdeContext _context;
        private readonly string _uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/comprobantes");

        public PagosController(RefugioVerdeContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var pagos = await _context.Pagos.Include(p => p.EstadoPago).Include(p => p.Reserva).ToListAsync();
            return View(pagos);
        }

        [HttpGet]
        public async Task<IActionResult> Listar()
        {
            var pagos = await _context.Pagos.Include(p => p.EstadoPago).Include(p => p.Reserva).ToListAsync();
            return Json(pagos);
        }

        [HttpGet]
        public async Task<IActionResult> Obtener(int id)
        {
            var pago = await _context.Pagos.Include(p => p.EstadoPago).Include(p => p.Reserva).FirstOrDefaultAsync(p => p.IdPago == id);
            if (pago == null)
            {
                return NotFound();
            }
            return Json(pago);
        }
        [HttpPost]
        public async Task<IActionResult> Crear([FromForm] Pago pago, [FromForm] IFormFile comprobante)
        {
            if (ModelState.IsValid)
            {
                if (comprobante != null && comprobante.Length > 0)
                {
                    var fileName = $"{Guid.NewGuid()}.jpeg";  // Usar un GUID como nombre del archivo para evitar colisiones
                    var filePath = Path.Combine(_uploadPath, fileName);

                    // Crear el directorio si no existe
                    if (!Directory.Exists(_uploadPath))
                    {
                        Directory.CreateDirectory(_uploadPath);
                    }

                    // Guardar la imagen en el servidor
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await comprobante.CopyToAsync(stream);
                    }

                    pago.Comprobante = filePath;  // Guardar la ruta del archivo en la base de datos
                }

                _context.Pagos.Add(pago);
                await _context.SaveChangesAsync();
                return Ok();
            }

            // Registrar los errores de validación
            var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
            foreach (var error in errors)
            {
                Console.WriteLine(error);
            }

            return BadRequest(ModelState);
        }

        [HttpPost]
        public async Task<IActionResult> Editar([FromForm] Pago pago)
        {
            if (ModelState.IsValid)
            {
                _context.Pagos.Update(pago);
                await _context.SaveChangesAsync();
                return Ok();
            }
            return BadRequest(ModelState);
        }

        [HttpDelete]
        public async Task<IActionResult> Eliminar(int id)
        {
            var pago = await _context.Pagos.FindAsync(id);
            if (pago == null)
            {
                return NotFound();
            }
            _context.Pagos.Remove(pago);
            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}