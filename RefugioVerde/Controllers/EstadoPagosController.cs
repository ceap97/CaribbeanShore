using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RefugioVerde.Models;

namespace RefugioVerde.Controllers
{
    public class EstadoPagosController : Controller
    {
        private readonly RefugioVerdeContext _context;

        public EstadoPagosController(RefugioVerdeContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var estadosPago = await _context.EstadoPagos.ToListAsync();
            return View(estadosPago);
        }

        [HttpGet]
        public async Task<IActionResult> Listar()
        {
            var estadosPago = await _context.EstadoPagos.ToListAsync();
            return Json(estadosPago);
        }

        // GET: /EstadoPagos/Obtener/{id}
        [HttpGet]
        public async Task<IActionResult> Obtener(int id)
        {
            var estadoPago = await _context.EstadoPagos.FindAsync(id);
            if (estadoPago == null)
            {
                return NotFound();
            }
            return Json(estadoPago);
        }

        // POST: /EstadoPagos/Crear
        [HttpPost]
        public async Task<IActionResult> Crear([FromForm] EstadoPago estadoPago)
        {
            if (ModelState.IsValid)
            {
                _context.EstadoPagos.Add(estadoPago);
                await _context.SaveChangesAsync();
                return Ok();
            }
            return BadRequest(ModelState);
        }

        // POST: /EstadoPagos/Editar
        [HttpPost]
        public async Task<IActionResult> Editar([FromForm] EstadoPago estadoPago)
        {
            if (ModelState.IsValid)
            {
                _context.EstadoPagos.Update(estadoPago);
                await _context.SaveChangesAsync();
                return Ok();
            }
            return BadRequest(ModelState);
        }

        // DELETE: /EstadoPagos/Eliminar/{id}
        [HttpDelete]
        public async Task<IActionResult> Eliminar(int id)
        {
            var estadoPago = await _context.EstadoPagos.FindAsync(id);
            if (estadoPago == null)
            {
                return NotFound();
            }
            _context.EstadoPagos.Remove(estadoPago);
            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}
