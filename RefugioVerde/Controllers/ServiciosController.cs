using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
        [HttpPost]
        public async Task<IActionResult> Crear([FromForm] Servicio servicio)
        {
            if (ModelState.IsValid)
            {
                _context.Servicios.Add(servicio);
                await _context.SaveChangesAsync();
                return Ok();
            }
            return BadRequest(ModelState);
        }

        // POST: /Servicios/Editar
        [HttpPost]
        public async Task<IActionResult> Editar([FromForm] Servicio servicio)
        {
            if (ModelState.IsValid)
            {
                _context.Servicios.Update(servicio);
                await _context.SaveChangesAsync();
                return Ok();
            }
            return BadRequest(ModelState);
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
