﻿using System;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RefugioVerde.Models;
using RefugioVerde.Recursos;
using MimeKit;
using MailKit.Net.Smtp;
using MailKit.Security;

namespace RefugioVerde.Controllers
{
    [Authorize]
    public class UsuariosController : Controller
    {
        private readonly RefugioVerdeContext _context;
        private readonly ILogger<UsuariosController> _logger;
        private readonly string _uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "usuarios");
        private readonly EmailService _emailService;
        private const int TOKEN_EXPIRY_MINUTES = 15;

        public UsuariosController(RefugioVerdeContext context, ILogger<UsuariosController> logger, EmailService emailService)
        {
            _context = context;
            _logger = logger;
            _emailService = emailService;
        }

        [HttpGet]
        public async Task<IActionResult> ObtenerUsuarioActual()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (int.TryParse(userId, out int parsedUserId))
            {
                var usuario = await _context.Usuarios
                    .Include(u => u.Empleado)
                    .FirstOrDefaultAsync(u => u.UsuarioId == parsedUserId);
                if (usuario == null)
                {
                    return NotFound();
                }
                return Json(usuario);
            }
            return BadRequest("Invalid user ID");
        }

        public IActionResult Dashboard()
        {
            ViewBag.TotalClientes = _context.Clientes.Count();
            ViewBag.TotalReservas = _context.Reservas.Count();
            ViewBag.TotalPagos = _context.Pagos.Count();

            // Datos para el gráfico de reservas por mes
            var reservasPorMes = _context.Reservas
                .GroupBy(r => new { r.FechaReserva.Year, r.FechaReserva.Month })
                .Select(g => new { Mes = g.Key.Month, Año = g.Key.Year, Total = g.Count() })
                .OrderBy(g => g.Año).ThenBy(g => g.Mes)
                .ToList();
            ViewBag.ReservasPorMesLabels = reservasPorMes.Select(r => $"{r.Mes}/{r.Año}").ToList();
            ViewBag.ReservasPorMesData = reservasPorMes.Select(r => r.Total).ToList();

            // Datos para el gráfico de reservas por estado
            var reservasPorEstado = _context.Reservas
                .GroupBy(r => r.EstadoReserva.Nombre)
                .Select(g => new { Estado = g.Key, Total = g.Count() })
                .ToList();
            ViewBag.ReservasPorEstadoLabels = reservasPorEstado.Select(r => r.Estado).ToList();
            ViewBag.ReservasPorEstadoData = reservasPorEstado.Select(r => r.Total).ToList();

            // Datos para el gráfico de pagos por estado
            var pagosPorEstado = _context.Pagos
                .GroupBy(p => p.EstadoPago.Nombre)
                .Select(g => new { Estado = g.Key, Total = g.Count() })
                .ToList();
            ViewBag.PagosPorEstadoLabels = pagosPorEstado.Select(p => p.Estado).ToList();
            ViewBag.PagosPorEstadoData = pagosPorEstado.Select(p => p.Total).ToList();

            // Datos para el gráfico de comparación de pagos, clientes, reservas y huéspedes
            ViewBag.ComparacionDatosLabels = new List<string> { "Pagos", "Clientes", "Reservas", "Huéspedes" };
            ViewBag.ComparacionDatosData = new List<int>
            {
                _context.Pagos.Count(),
                _context.Clientes.Count(),
                _context.Reservas.Count(),
                _context.Huespeds.Count()
            };

            return View();
        }

        public async Task<IActionResult> Index()
        {
            var usuarios = await _context.Usuarios.Include(u => u.Empleado).ToListAsync();
            var empleados = await _context.Empleados.ToListAsync();
            ViewBag.Empleados = empleados;
            return View(usuarios);
        }

        [HttpGet]
        public async Task<IActionResult> Listar()
        {
            var usuarios = await _context.Usuarios.Include(u => u.Empleado).ToListAsync();
            return Json(usuarios);
        }

        [HttpGet]
        public async Task<IActionResult> Obtener(int id)
        {
            var usuario = await _context.Usuarios.Include(u => u.Empleado).FirstOrDefaultAsync(u => u.UsuarioId == id);
            if (usuario == null)
            {
                return NotFound();
            }
            return Json(usuario);
        }

        [HttpPost]
        public async Task<IActionResult> Crear([FromForm] Usuario usuario, [FromForm] IFormFile imagen)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var usuarioExistente = await _context.Usuarios.FirstOrDefaultAsync(u => u.Correo == usuario.Correo);
                    if (usuarioExistente != null)
                    {
                        return BadRequest(new { message = "El correo ya está registrado. Intente con otro." });
                    }

                    if (imagen != null && imagen.Length > 0)
                    {
                        var fileName = $"{usuario.NombreUsuario}.jpeg";
                        var filePath = Path.Combine(_uploadPath, fileName);

                        if (!Directory.Exists(_uploadPath))
                        {
                            Directory.CreateDirectory(_uploadPath);
                        }

                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await imagen.CopyToAsync(stream);
                        }

                        usuario.Imagen = $"/images/usuarios/{fileName}";
                    }
                    else
                    {
                        // Asignar imagen por defecto
                        usuario.Imagen = "/images/usuarios/default.jpeg";
                    }

                    usuario.Clave = Utilidades.EncriptarClave(usuario.Clave);
                    _context.Usuarios.Add(usuario);
                    await _context.SaveChangesAsync();
                    return Ok();
                }
                return BadRequest(ModelState);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ocurrió un error al crear el usuario.");
                return StatusCode(500, "Ocurrió un error en el servidor.");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Editar([FromForm] Usuario usuario, [FromForm] IFormFile imagen)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var usuarioExistente = await _context.Usuarios.FirstOrDefaultAsync(u => u.Correo == usuario.Correo && u.UsuarioId != usuario.UsuarioId);
                    if (usuarioExistente != null)
                    {
                        return BadRequest(new { message = "El correo ya está registrado. Intente con otro." });
                    }

                    var usuarioDb = await _context.Usuarios.FindAsync(usuario.UsuarioId);
                    if (usuarioDb == null)
                    {
                        return NotFound();
                    }

                    usuarioDb.NombreUsuario = usuario.NombreUsuario;
                    usuarioDb.Correo = usuario.Correo;
                    if (!string.IsNullOrEmpty(usuario.Clave))
                    {
                        usuarioDb.Clave = Utilidades.EncriptarClave(usuario.Clave);
                    }
                    usuarioDb.EmpleadoId = usuario.EmpleadoId;

                    if (imagen != null && imagen.Length > 0)
                    {
                        var fileName = $"{usuario.NombreUsuario}.jpeg";
                        var filePath = Path.Combine(_uploadPath, fileName);

                        if (!Directory.Exists(_uploadPath))
                        {
                            Directory.CreateDirectory(_uploadPath);
                        }

                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await imagen.CopyToAsync(stream);
                        }

                        usuarioDb.Imagen = $"/images/usuarios/{fileName}";
                    }
                    else if (string.IsNullOrEmpty(usuarioDb.Imagen))
                    {
                        // Asignar imagen por defecto si no tiene una
                        usuarioDb.Imagen = "/images/usuarios/default.jpeg";
                    }

                    _context.Usuarios.Update(usuarioDb);
                    await _context.SaveChangesAsync();
                    return Ok();
                }
                return BadRequest(ModelState);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ocurrió un error al editar el usuario.");
                return StatusCode(500, "Ocurrió un error en el servidor.");
            }
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> SolicitarRestablecerContraseña([FromForm] string correo)
        {
            var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.Correo == correo);
            if (usuario == null)
            {
                return BadRequest(new { message = "No existe una cuenta con este correo." });
            }

            // Generar token aleatorio
            var token = Guid.NewGuid().ToString("N");

            // Guardar token en TempData (en producción usar base de datos)
            TempData[$"ResetToken_{correo}"] = token;
            TempData[$"ResetTokenExpiry_{correo}"] = DateTime.UtcNow.AddMinutes(TOKEN_EXPIRY_MINUTES);

            await _emailService.SendPasswordResetEmailAsync(correo, token);

            return Ok(new { message = "Se ha enviado un correo con las instrucciones." });
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> RestablecerContraseña([FromForm] string correo,
            [FromForm] string token, [FromForm] string nuevaClave)
        {
            var storedToken = TempData[$"ResetToken_{correo}"]?.ToString();
            var expiryStr = TempData[$"ResetTokenExpiry_{correo}"]?.ToString();

            if (string.IsNullOrEmpty(storedToken) || string.IsNullOrEmpty(expiryStr))
            {
                return BadRequest(new { message = "Token inválido o expirado." });
            }

            if (DateTime.Parse(expiryStr) < DateTime.UtcNow)
            {
                return BadRequest(new { message = "El token ha expirado." });
            }

            if (token != storedToken)
            {
                return BadRequest(new { message = "Token inválido." });
            }

            var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.Correo == correo);
            if (usuario == null)
            {
                return BadRequest(new { message = "Usuario no encontrado." });
            }

            usuario.Clave = Utilidades.EncriptarClave(nuevaClave);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Contraseña actualizada correctamente." });
        }

        // DELETE: /Usuarios/Eliminar/{id}
        [HttpDelete]
        public async Task<IActionResult> Eliminar(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null)
            {
                return NotFound();
            }

            // Eliminar la imagen del servidor si existe
            if (!string.IsNullOrEmpty(usuario.Imagen))
            {
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", usuario.Imagen.TrimStart('/'));
                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);  // Eliminar archivo
                }
            }

            _context.Usuarios.Remove(usuario);
            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}

public class EmailService
{
    private readonly IConfiguration _configuration;

    public EmailService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task SendPasswordResetEmailAsync(string toEmail, string resetToken)
    {
        var email = new MimeMessage();
        email.From.Add(MailboxAddress.Parse(_configuration["EmailSettings:Mail"]));
        email.To.Add(MailboxAddress.Parse(toEmail));
        email.Subject = "Restablecer Contraseña - Refugio Verde";

        var builder = new BodyBuilder();
        builder.HtmlBody = $@"
            <h1>Restablecer Contraseña</h1>
            <p>Use este token para restablecer su contraseña: {resetToken}</p>
            <p>Este token expirará en 15 minutos.</p>";

        email.Body = builder.ToMessageBody();

        using var smtp = new SmtpClient();
        await smtp.ConnectAsync(_configuration["EmailSettings:Host"],
            int.Parse(_configuration["EmailSettings:Port"]), SecureSocketOptions.StartTls);
        await smtp.AuthenticateAsync(_configuration["EmailSettings:Mail"],
            _configuration["EmailSettings:Password"]);
        await smtp.SendAsync(email);
        await smtp.DisconnectAsync(true);
    }
}
