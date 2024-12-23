﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Claims;
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

        [HttpPost]
        public async Task<IActionResult> CambiarEstado([FromBody] CambioEstadoPagoRequest request)
        {
            try
            {
                var pago = await _context.Pagos.FindAsync(request.IdPago);
                if (pago == null)
                {
                    return NotFound(new { message = "Pago no encontrado" });
                }

                var estadoPago = await _context.EstadoPagos.FindAsync(request.EstadoPagoId);
                if (estadoPago == null)
                {
                    return NotFound(new { message = "Estado de pago no encontrado" });
                }

                pago.EstadoPagoId = request.EstadoPagoId;
                await _context.SaveChangesAsync();

                // Si el estado del pago es "Aprobado", cambiar el estado de la reserva a "Confirmada"
                if (estadoPago.Nombre == "Aprobado")
                {
                    var reserva = await _context.Reservas.FindAsync(pago.ReservaId);
                    if (reserva != null)
                    {
                        var estadoConfirmada = await _context.EstadoReservas.FirstOrDefaultAsync(e => e.Nombre == "Confirmada");
                        if (estadoConfirmada != null)
                        {
                            reserva.EstadoReservaId = estadoConfirmada.EstadoReservaId;
                            await _context.SaveChangesAsync();
                        }
                    }
                }

                return Ok(new { message = "Estado del pago actualizado exitosamente" });
            }
            catch (Exception ex)
            {
                // Log the exception (you can use a logging framework like Serilog, NLog, etc.)
                Console.WriteLine(ex.Message);
                return StatusCode(500, new { message = "Error interno del servidor", details = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Crear([FromForm] Pago pago, [FromForm] IFormFile comprobante)
        {
            // Validar archivo
            if (comprobante == null || comprobante.Length == 0)
            {
                ModelState.AddModelError("Comprobante", "El comprobante es requerido");
                return BadRequest(new { errors = new[] { "El comprobante es requerido" } });
            }

            // Validar tipo de archivo
            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png" };
            var extension = Path.GetExtension(comprobante.FileName).ToLowerInvariant();
            if (!allowedExtensions.Contains(extension))
            {
                ModelState.AddModelError("Comprobante", "Solo se permiten archivos JPG, JPEG o PNG");
                return BadRequest(new { errors = new[] { "Formato de archivo no válido" } });
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var fileName = $"{Guid.NewGuid()}{extension}";
                    var filePath = Path.Combine(_uploadPath, fileName);

                    if (!Directory.Exists(_uploadPath))
                    {
                        Directory.CreateDirectory(_uploadPath);
                    }

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await comprobante.CopyToAsync(stream);
                    }

                    pago.Comprobante = fileName;

                    // Establecer el estado de pago a "En revisión"
                    var estadoEnRevision = await _context.EstadoPagos.FirstOrDefaultAsync(e => e.Nombre == "En revisión");
                    if (estadoEnRevision == null)
                    {
                        return BadRequest(new { errors = new[] { "Estado de pago 'En revisión' no encontrado" } });
                    }
                    pago.EstadoPagoId = estadoEnRevision.EstadoPagoId;

                    _context.Pagos.Add(pago);
                    await _context.SaveChangesAsync();

                    // Si el estado del pago es "Aprobado", cambiar el estado de la reserva a "Confirmada"
                    if (estadoEnRevision.Nombre == "Aprobado")
                    {
                        var reserva = await _context.Reservas.FindAsync(pago.ReservaId);
                        if (reserva != null)
                        {
                            var estadoConfirmada = await _context.EstadoReservas.FirstOrDefaultAsync(e => e.Nombre == "Confirmada");
                            if (estadoConfirmada != null)
                            {
                                reserva.EstadoReservaId = estadoConfirmada.EstadoReservaId;
                                await _context.SaveChangesAsync();
                            }
                        }
                    }

                    return Ok(new { message = "Pago creado exitosamente", montoFormateado = pago.MontoFormateado });
                }
                catch (DbUpdateException ex)
                {
                    // Log the error (uncomment ex variable name and write a log.)
                    return BadRequest(new { errors = new[] { "Error al procesar el pago: " + ex.InnerException?.Message } });
                }
                catch (Exception ex)
                {
                    return BadRequest(new { errors = new[] { "Error al procesar el pago: " + ex.Message } });
                }
            }

            var errors = ModelState.Values
                .SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage)
                .ToList();
            return BadRequest(new { errors });
        }

        public async Task<IActionResult> Index()
        {
            var pagos = await _context.Pagos.Include(p => p.EstadoPago).Include(p => p.Reserva).Include(m => m.MetodoDePago).ToListAsync();
            return View(pagos);
        }

        [HttpGet]
        public async Task<IActionResult> Listar()
        {
            var pagos = await _context.Pagos.Include(p => p.EstadoPago).Include(p => p.Reserva).Include(m => m.MetodoDePago).ToListAsync();
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
        public async Task<IActionResult> Editar([FromForm] Pago pago, [FromForm] IFormFile comprobante)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (comprobante != null && comprobante.Length > 0)
                    {
                        // Validar tipo de archivo
                        var allowedExtensions = new[] { ".jpg", ".jpeg", ".png" };
                        var extension = Path.GetExtension(comprobante.FileName).ToLowerInvariant();
                        if (!allowedExtensions.Contains(extension))
                        {
                            ModelState.AddModelError("Comprobante", "Solo se permiten archivos JPG, JPEG o PNG");
                            return BadRequest(new { errors = new[] { "Formato de archivo no válido" } });
                        }

                        var fileName = $"{Guid.NewGuid()}{extension}";
                        var filePath = Path.Combine(_uploadPath, fileName);

                        if (!Directory.Exists(_uploadPath))
                        {
                            Directory.CreateDirectory(_uploadPath);
                        }

                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await comprobante.CopyToAsync(stream);
                        }

                        pago.Comprobante = fileName;
                    }

                    _context.Pagos.Update(pago);
                    await _context.SaveChangesAsync();

                    // Si el estado del pago es "Aprobado", cambiar el estado de la reserva a "Confirmada"
                    var estadoPago = await _context.EstadoPagos.FindAsync(pago.EstadoPagoId);
                    if (estadoPago != null && estadoPago.Nombre == "Aprobado")
                    {
                        var reserva = await _context.Reservas.FindAsync(pago.ReservaId);
                        if (reserva != null)
                        {
                            var estadoConfirmada = await _context.EstadoReservas.FirstOrDefaultAsync(e => e.Nombre == "Confirmada");
                            if (estadoConfirmada != null)
                            {
                                reserva.EstadoReservaId = estadoConfirmada.EstadoReservaId;
                                await _context.SaveChangesAsync();
                            }
                        }
                    }

                    return Ok(new { message = "Pago editado exitosamente", montoFormateado = pago.MontoFormateado });
                }
                catch (DbUpdateException ex)
                {
                    return BadRequest(new { errors = new[] { "Error al procesar el pago: " + ex.InnerException?.Message } });
                }
                catch (Exception ex)
                {
                    return BadRequest(new { errors = new[] { "Error al procesar el pago: " + ex.Message } });
                }
            }

            var errors = ModelState.Values
                .SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage)
                .ToList();
            return BadRequest(new { errors });
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

        [HttpGet]
        public async Task<IActionResult> ListarMetodosDePago()
        {
            var metodosDePago = await _context.MetodoDePagos.ToListAsync();
            return Json(metodosDePago);
        }

        [HttpGet]
        public async Task<IActionResult> ListarEstadosPago()
        {
            var estadosPago = await _context.EstadoPagos.ToListAsync();
            return Json(estadosPago);
        }

        [HttpGet]
        public async Task<IActionResult> ListarPagosClientes()
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (int.TryParse(userId, out int parsedUserId))
                {
                    var cliente = await _context.Clientes
                        .Include(c => c.Reservas)
                            .ThenInclude(r => r.Pagos)
                                .ThenInclude(p => p.EstadoPago)
                        .Include(c => c.Reservas)
                            .ThenInclude(r => r.Pagos)
                                .ThenInclude(p => p.MetodoDePago)
                        .FirstOrDefaultAsync(c => c.UsuarioId == parsedUserId);

                    if (cliente == null)
                    {
                        return NotFound();
                    }

                    var pagosViewModel = cliente.Reservas
                        .SelectMany(r => r.Pagos)
                        .Select(p => new PagosViewModel
                        {
                            IdPago = p.IdPago,
                            Monto = p.Monto,
                            MetodoDePagoId = p.MetodoDePagoId,
                            Comprobante = p.Comprobante,
                            ReservaId = p.ReservaId,
                            EstadoPagoId = p.EstadoPagoId,
                            Tipo = p.Tipo,
                            FechaPago = p.FechaPago,
                            EstadoPagoNombre = p.EstadoPago?.Nombre,
                            MetodoDePagoNombre = p.MetodoDePago?.Nombre,
                            ReservaConfirmacion = p.Reserva?.Confirmacion
                        }).ToList();

                    return View(pagosViewModel);
                }
                return BadRequest("Invalid user ID");
            }
            catch (Exception ex)
            {
                // Log the exception (you can use a logging framework like Serilog, NLog, etc.)
                Console.WriteLine(ex.Message);
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
