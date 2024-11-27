using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
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
        [HttpGet]
        public async Task<IActionResult> ListarReservasCliente()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (int.TryParse(userId, out int parsedUserId))
            {
                var cliente = await _context.Clientes
                    .Include(c => c.Reservas)
                        .ThenInclude(r => r.Habitacion)
                    .Include(c => c.Reservas)
                        .ThenInclude(r => r.EstadoReserva)
                    .Include(c => c.Reservas)
                        .ThenInclude(r => r.Servicio)
                    .Include(c => c.Reservas)
                        .ThenInclude(r => r.Comodidad)
                    .FirstOrDefaultAsync(c => c.UsuarioId == parsedUserId);

                if (cliente == null)
                {
                    return NotFound();
                }

                var reservasViewModel = cliente.Reservas.Select(r => new ReservaViewModel
                {
                    ReservaId = r.ReservaId,
                    FechaReserva = r.FechaReserva,
                    FechaInicio = r.FechaInicio,
                    FechaFin = r.FechaFin,
                    Habitacion = r.Habitacion?.NombreHabitacion,
                    EstadoReserva = r.EstadoReserva?.Nombre,
                    Servicio = r.Servicio?.Nombre,
                    Comodidad = r.Comodidad?.Nombre
                }).ToList();

                return View(reservasViewModel);
            }
            return BadRequest("Invalid user ID");
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
