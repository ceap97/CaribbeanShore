using System;
using System.Collections.Generic;
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

        // GET: /Pagos/Obtener/{id}
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

        // POST: /Pagos/Crear
        [HttpPost]
        public async Task<IActionResult> Crear([FromForm] Pago pago)
        {
            if (ModelState.IsValid)
            {
                _context.Pagos.Add(pago);
                await _context.SaveChangesAsync();
                return Ok();
            }
            return BadRequest(ModelState);
        }

        // POST: /Pagos/Editar
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

        // DELETE: /Pagos/Eliminar/{id}
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
