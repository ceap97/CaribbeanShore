using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RefugioVerde.Models;
using RefugioVerde.Recursos;

namespace RefugioVerde.Controllers
{
    [Authorize]
    public class UsuariosController : Controller
    {
        private readonly RefugioVerdeContext _context;
        private readonly string _uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "usuarios");

        public UsuariosController(RefugioVerdeContext context)
        {
            _context = context;
        }
        private int? GetLoggedInUserId()
        {
            if (Request.Cookies.TryGetValue("userId", out string userId))
            {
                if (int.TryParse(userId, out int id))
                {
                    return id;
                }
            }
            return null;
        }
        public IActionResult Dashboard()
        {
            ViewBag.EmpleadosData = new
            {
                labels = new[] { "Empleados" },
                data = new[] { _context.Empleados.Count() }
            };

            ViewBag.UsuariosData = new
            {
                labels = new[] { "Usuarios Registrados" },
                data = new[] { _context.Usuarios.Count() }
            };

            ViewBag.HuespedesData = new
            {
                labels = new[] { "Huéspedes" },
                data = new[] { _context.Huespeds.Count() }
            };

            ViewBag.HabitacionesData = new
            {
                labels = new[] { "Reservadas", "Disponibles" },
                data = new[] {
                    _context.Habitacions.Count(h => h.EstadoHabitacionId == 2), 
                    _context.Habitacions.Count(h => h.EstadoHabitacionId == 1)  
                }
            };

            ViewBag.ClientesReservasData = new
            {
                labels = new[] { "Clientes con Reservas" },
                data = new[] { _context.Clientes.Count(c => c.Reservas.Any()) }
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

        // POST: /Usuarios/Crear
        [HttpPost]
        public async Task<IActionResult> Crear([FromForm] Usuario usuario, [FromForm] IFormFile imagen)
        {
            if (ModelState.IsValid)
            {
                // Encriptar la contraseña antes de guardar
                usuario.Clave = Utilidades.EncriptarClave(usuario.Clave);

                // Si se proporciona una imagen, se guarda en el servidor
                if (imagen != null && imagen.Length > 0)
                {
                    var fileName = $"{usuario.NombreUsuario}.jpeg";  // Usar el nombre de usuario como nombre del archivo
                    var filePath = Path.Combine(_uploadPath, fileName);

                    // Crear el directorio si no existe
                    if (!Directory.Exists(_uploadPath))
                    {
                        Directory.CreateDirectory(_uploadPath);
                    }

                    // Guardar la imagen en el servidor
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await imagen.CopyToAsync(stream);
                    }

                    usuario.Imagen = $"/images/usuarios/{fileName}";  // Guardar la ruta relativa en la base de datos
                }

                _context.Usuarios.Add(usuario);
                await _context.SaveChangesAsync();
                return Ok();
            }
            return BadRequest(ModelState);
        }

        // POST: /Usuarios/Editar
        [HttpPost]
        public async Task<IActionResult> Editar([FromForm] Usuario usuario, [FromForm] IFormFile imagen)
        {
            if (ModelState.IsValid)
            {
                // Si la clave tiene un valor (es decir, el usuario está cambiando la contraseña)
                if (!string.IsNullOrEmpty(usuario.Clave))
                {
                    // Encriptar la contraseña antes de guardar
                    usuario.Clave = Utilidades.EncriptarClave(usuario.Clave);
                }

                // Si se proporciona una nueva imagen, se guarda en el servidor
                if (imagen != null && imagen.Length > 0)
                {
                    var fileName = $"{usuario.NombreUsuario}.jpeg";  // Usar el nombre de usuario como nombre del archivo
                    var filePath = Path.Combine(_uploadPath, fileName);

                    // Crear el directorio si no existe
                    if (!Directory.Exists(_uploadPath))
                    {
                        Directory.CreateDirectory(_uploadPath);
                    }

                    // Guardar la nueva imagen
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await imagen.CopyToAsync(stream);
                    }

                    usuario.Imagen = $"/images/usuarios/{fileName}";  // Guardar la ruta relativa en la base de datos
                }
                else
                {
                    // Si no se proporciona una nueva imagen, mantener la imagen existente
                    var existingUsuario = await _context.Usuarios.AsNoTracking().FirstOrDefaultAsync(u => u.UsuarioId == usuario.UsuarioId);
                    if (existingUsuario != null)
                    {
                        usuario.Imagen = existingUsuario.Imagen;
                    }
                }

                _context.Usuarios.Update(usuario);
                await _context.SaveChangesAsync();
                return Ok();
            }
            return BadRequest(ModelState);
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
