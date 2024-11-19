using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RefugioVerde.Models;

namespace RefugioVerde.Controllers
{
    public class ReservasController : Controller
    {
        private readonly RefugioVerdeContext _context;

        public ReservasController(RefugioVerdeContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var reservas = await _context.Reservas
                .Include(r => r.Cliente)
                .Include(r => r.Habitacion)
                .Include(r => r.EstadoReserva)
                .Include(r => r.Comodidad)
                .Include(r => r.Servicio)
                .ToListAsync();
            return View(reservas);
        }

        [HttpGet]
        public async Task<IActionResult> Listar()
        {
            var reservas = await _context.Reservas
                .Include(r => r.Cliente)
                .Include(r => r.Habitacion)
                .Include(r => r.EstadoReserva)
                .Include(r => r.Comodidad)
                .Include(r => r.Servicio)
                .ToListAsync();
            return Json(reservas);
        }

        // GET: /Reservas/Obtener/{id}
        [HttpGet]
        public async Task<IActionResult> Obtener(int id)
        {
            var reserva = await _context.Reservas
                .Include(r => r.Cliente)
                .Include(r => r.Habitacion)
                .Include(r => r.EstadoReserva)
                .Include(r => r.Comodidad)
                .Include(r => r.Servicio)
                .FirstOrDefaultAsync(r => r.ReservaId == id);
            if (reserva == null)
            {
                return NotFound();
            }
            return Json(reserva);
        }

        // POST: /Reservas/Crear
        [HttpPost]
        public async Task<IActionResult> Crear([FromForm] Reserva reserva)
        {
            if (ModelState.IsValid)
            {
                _context.Reservas.Add(reserva);
                await _context.SaveChangesAsync();
                return Ok();
            }
            return BadRequest(ModelState);
        }

        // POST: /Reservas/Editar
        [HttpPost]
        public async Task<IActionResult> Editar([FromForm] Reserva reserva)
        {
            if (ModelState.IsValid)
            {
                _context.Reservas.Update(reserva);
                await _context.SaveChangesAsync();
                return Ok();
            }
            return BadRequest(ModelState);
        }

        // DELETE: /Reservas/Eliminar/{id}
        [HttpDelete]
        public async Task<IActionResult> Eliminar(int id)
        {
            var reserva = await _context.Reservas.FindAsync(id);
            if (reserva == null)
            {
                return NotFound();
            }
            _context.Reservas.Remove(reserva);
            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}
