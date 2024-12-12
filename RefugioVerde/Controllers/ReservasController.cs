using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RefugioVerde.Models;
using System.Globalization;

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
        public async Task<IActionResult> VerificarDisponibilidad(int habitacionId, DateTime fechaInicio, DateTime fechaFin)
        {
            var reservasExistentes = await _context.Reservas
                .Where(r => r.HabitacionId == habitacionId &&
                            ((fechaInicio >= r.FechaInicio && fechaInicio < r.FechaFin) ||
                             (fechaFin > r.FechaInicio && fechaFin <= r.FechaFin) ||
                             (fechaInicio <= r.FechaInicio && fechaFin >= r.FechaFin)))
                .ToListAsync();

            if (reservasExistentes.Any())
            {
                return Json(new { disponible = false });
            }

            return Json(new { disponible = true });
        }

        [HttpPost]
        public async Task<IActionResult> CambiarEstadoReserva(int reservaId, string nuevoEstado)
        {
            var reserva = await _context.Reservas.FindAsync(reservaId);
            if (reserva == null)
            {
                return NotFound(new { message = "Reserva no encontrada" });
            }

            var estadoReserva = await _context.EstadoReservas.FirstOrDefaultAsync(e => e.Nombre == nuevoEstado);
            if (estadoReserva == null)
            {
                return NotFound(new { message = "Estado de reserva no encontrado" });
            }

            reserva.EstadoReservaId = estadoReserva.EstadoReservaId;
            await _context.SaveChangesAsync();

            return Ok(new { message = "Estado de la reserva actualizado exitosamente" });
        }
    
        [HttpPost]
        public async Task<IActionResult> VerificarReservasNoPagadas()
        {
            var reservasNoPagadas = await _context.Reservas
                .Include(r => r.EstadoReserva)
                .Where(r => r.EstadoReserva.Nombre == "Pendiente" && r.FechaReserva.AddHours(1) <= DateTime.Now)
                .ToListAsync();

            var estadoCancelada = await _context.EstadoReservas
                .FirstOrDefaultAsync(e => e.Nombre == "Cancelada");

            if (estadoCancelada == null)
            {
                return NotFound("Estado 'Cancelada' no encontrado");
            }

            foreach (var reserva in reservasNoPagadas)
            {
                reserva.EstadoReserva = estadoCancelada;
            }

            await _context.SaveChangesAsync();
            return Ok();
        }


        [HttpPost]
        public IActionResult CancelarReserva(int id)
        {
            var reserva = _context.Reservas
                .Include(r => r.EstadoReserva)
                .FirstOrDefault(r => r.ReservaId == id);
            if (reserva == null)
            {
                return NotFound();
            }

            var estadoCancelada = _context.EstadoReservas
                .FirstOrDefault(e => e.Nombre == "Cancelada");
            if (estadoCancelada == null)
            {
                return NotFound("Estado 'Cancelada' no encontrado");
            }

            reserva.EstadoReserva = estadoCancelada;
            _context.SaveChanges();

            return Ok();
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
                        .ThenInclude(r => r.Servicios)
                    .Include(c => c.Reservas)
                        .ThenInclude(r => r.Comodidades)
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
                    Servicios = r.Servicios.Select(s => s.Nombre).ToList(),
                    Comodidades = r.Comodidades.Select(c => c.Nombre).ToList(),
                    PrecioHabitacion = r.Habitacion?.Precio ?? 0,
                    PrecioComodidad = r.Comodidades.Sum(c => c.Precio),
                    PrecioServicio = r.Servicios.Sum(s => s.Precio),
                    MontoTotal = r.MontoTotal,
                    Confirmacion = r.Confirmacion
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
                .Include(r => r.Comodidades)
                .Include(r => r.Servicios)
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
                .Include(r => r.Comodidades)
                .Include(r => r.Servicios)
                .ToListAsync();
            return Json(reservas);
        }

         [HttpGet]
        public async Task<IActionResult> Obtener(int id)
        {
            var reserva = await _context.Reservas
                .Include(r => r.Cliente)
                .Include(r => r.Habitacion)
                .Include(r => r.EstadoReserva)
                .FirstOrDefaultAsync(r => r.ReservaId == id);

            if (reserva == null)
            {
                return NotFound();
            }

            return Json(new
            {
                reserva.ReservaId,
                reserva.FechaReserva,
                reserva.ClienteId,
                reserva.HabitacionId,
                reserva.EstadoReservaId,
                reserva.FechaInicio,
                reserva.FechaFin,
                reserva.MontoTotal,
                MontoTotalFormateado = reserva.MontoTotalFormateado,
                reserva.Confirmacion
            });
        }

        [HttpPost]
        public async Task<IActionResult> Crear([FromForm] Reserva reserva)
        {
            if (reserva.FechaFin <= reserva.FechaInicio)
            {
                ModelState.AddModelError("FechaFin", "La fecha de fin debe ser mayor que la fecha de inicio.");
            }

            // Verificar disponibilidad de la habitación
            var reservasExistentes = await _context.Reservas
                .Where(r => r.HabitacionId == reserva.HabitacionId &&
                            ((reserva.FechaInicio >= r.FechaInicio && reserva.FechaInicio < r.FechaFin) ||
                             (reserva.FechaFin > r.FechaInicio && reserva.FechaFin <= r.FechaFin) ||
                             (reserva.FechaInicio <= r.FechaInicio && reserva.FechaFin >= r.FechaFin)))
                .ToListAsync();

            if (reservasExistentes.Any())
            {
                ModelState.AddModelError("FechaInicio", "La habitación no está disponible en las fechas seleccionadas.");
            }

            if (ModelState.IsValid)
            {
                reserva.Comodidades = new List<Comodidad>();
                foreach (var comodidadId in reserva.ComodidadesSeleccionadas)
                {
                    var comodidad = await _context.Comodidads.FindAsync(comodidadId);
                    if (comodidad != null)
                    {
                        reserva.Comodidades.Add(comodidad);
                    }
                }

                reserva.Servicios = new List<Servicio>();
                foreach (var servicioId in reserva.ServiciosSeleccionados)
                {
                    var servicio = await _context.Servicios.FindAsync(servicioId);
                    if (servicio != null)
                    {
                        reserva.Servicios.Add(servicio);
                    }
                }

                // Calcular el monto total
                var precioHabitacion = (await _context.Habitacions.FindAsync(reserva.HabitacionId))?.Precio ?? 0;
                var precioComodidades = reserva.Comodidades.Sum(c => c.Precio);
                var precioServicios = reserva.Servicios.Sum(s => s.Precio);
                var numeroDias = (reserva.FechaFin - reserva.FechaInicio).Days;

                reserva.MontoTotal = (precioHabitacion + precioComodidades + precioServicios) * numeroDias;

                _context.Reservas.Add(reserva);
                await _context.SaveChangesAsync();

                // Generar la confirmación
                reserva.Confirmacion = $"{reserva.ReservaId}{reserva.FechaReserva:yyyyMMdd}";
                await _context.SaveChangesAsync();

                return Ok(new { confirmacion = reserva.Confirmacion });
            }
            return BadRequest(ModelState);
        }


        // POST: /Reservas/Editar
        [HttpPost]
        public async Task<IActionResult> Editar([FromForm] Reserva reserva)
        {
            if (reserva.FechaFin <= reserva.FechaInicio)
            {
                ModelState.AddModelError("FechaFin", "La fecha de fin debe ser mayor que la fecha de inicio.");
            }

            if (ModelState.IsValid)
            {
                var existingReserva = await _context.Reservas
                    .Include(r => r.Comodidades)
                    .Include(r => r.Servicios)
                    .FirstOrDefaultAsync(r => r.ReservaId == reserva.ReservaId);

                if (existingReserva != null)
                {
                    existingReserva.FechaInicio = reserva.FechaInicio;
                    existingReserva.FechaFin = reserva.FechaFin;
                    existingReserva.ClienteId = reserva.ClienteId;
                    existingReserva.HabitacionId = reserva.HabitacionId;
                    existingReserva.EstadoReservaId = reserva.EstadoReservaId;

                    existingReserva.Comodidades.Clear();
                    foreach (var comodidadId in reserva.ComodidadesSeleccionadas)
                    {
                        var comodidad = await _context.Comodidads.FindAsync(comodidadId);
                        if (comodidad != null)
                        {
                            existingReserva.Comodidades.Add(comodidad);
                        }
                    }

                    existingReserva.Servicios.Clear();
                    foreach (var servicioId in reserva.ServiciosSeleccionados)
                    {
                        var servicio = await _context.Servicios.FindAsync(servicioId);
                        if (servicio != null)
                        {
                            existingReserva.Servicios.Add(servicio);
                        }
                    }

                    // Recalcular el monto total
                    var precioHabitacion = (await _context.Habitacions.FindAsync(existingReserva.HabitacionId))?.Precio ?? 0;
                    var precioComodidades = existingReserva.Comodidades.Sum(c => c.Precio);
                    var precioServicios = existingReserva.Servicios.Sum(s => s.Precio);
                    var numeroDias = (existingReserva.FechaFin - existingReserva.FechaInicio).Days;

                    existingReserva.MontoTotal = (precioHabitacion + precioComodidades + precioServicios) * numeroDias;

                    _context.Reservas.Update(existingReserva);
                    await _context.SaveChangesAsync();
                    return Ok();
                }
                return NotFound();
            }
            return BadRequest(ModelState);
        }

        // DELETE: /Reservas/Eliminar/{id}
        [HttpDelete]
        public async Task<IActionResult> Eliminar(int id)
        {
            var reserva = await _context.Reservas
                .Include(r => r.Comodidades)
                .Include(r => r.Servicios)
                .FirstOrDefaultAsync(r => r.ReservaId == id);
            if (reserva == null)
            {
                return NotFound();
            }

            // Eliminar las relaciones con comodidades y servicios
            reserva.Comodidades.Clear();
            reserva.Servicios.Clear();

            _context.Reservas.Remove(reserva);
            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}

