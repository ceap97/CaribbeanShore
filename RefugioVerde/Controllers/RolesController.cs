using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RefugioVerde.Models;


namespace RefugioVerde.Controllers
{

    public class RolesController : Controller
    {
        private readonly RefugioVerdeContext _context;

        public RolesController(RefugioVerdeContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var roles = await _context.Roles.ToListAsync();
            var permisos = await _context.Permisos.ToListAsync();
            ViewBag.Permisos = permisos;
            return View(roles);
        }

        [HttpGet]
        public async Task<IActionResult> Listar()
        {
            var roles = await _context.Roles.ToListAsync();
            return Json(roles);
        }

        [HttpGet]
        public async Task<IActionResult> Obtener(int id)
        {
            var role = await _context.Roles
                .Include(r => r.Permisos)
                .FirstOrDefaultAsync(r => r.RolId == id);
            if (role == null)
            {
                return NotFound();
            }
            role.PermisosSeleccionados = role.Permisos.Select(p => p.PermisoId).ToList();
            return Json(role);
        }

        [HttpGet]
        public async Task<IActionResult> Detalles(int id)
        {
            var role = await _context.Roles.FirstOrDefaultAsync(r => r.RolId == id);
            if (role == null)
            {
                return NotFound();
            }
            return View(role);
        }

        [HttpPost]
        public async Task<IActionResult> Crear([FromForm] Role role)
        {
            if (ModelState.IsValid)
            {
                _context.Roles.Add(role);
                await _context.SaveChangesAsync();

                // Agregar permisos seleccionados
                foreach (var permisoId in role.PermisosSeleccionados)
                {
                    var permiso = await _context.Permisos.FindAsync(permisoId);
                    if (permiso != null)
                    {
                        role.Permisos.Add(permiso);
                    }
                }
                await _context.SaveChangesAsync();

                return Ok();
            }
            return BadRequest(ModelState);
        }

        [HttpPost]
        public async Task<IActionResult> Editar([FromForm] Role role)
        {
            if (ModelState.IsValid)
            {
                var existingRole = await _context.Roles
                    .Include(r => r.Permisos)
                    .FirstOrDefaultAsync(r => r.RolId == role.RolId);

                if (existingRole != null)
                {
                    existingRole.Nombre = role.Nombre;
                    existingRole.Permisos.Clear();

                    // Agregar permisos seleccionados
                    foreach (var permisoId in role.PermisosSeleccionados)
                    {
                        var permiso = await _context.Permisos.FindAsync(permisoId);
                        if (permiso != null)
                        {
                            existingRole.Permisos.Add(permiso);
                        }
                    }

                    _context.Roles.Update(existingRole);
                    await _context.SaveChangesAsync();
                    return Ok();
                }
            }
            return BadRequest(ModelState);
        }
        [HttpDelete]
        public async Task<IActionResult> Eliminar(int id)
        {
            try
            {
                var role = await _context.Roles
                    .Include(r => r.Permisos)
                    .FirstOrDefaultAsync(r => r.RolId == id);
                if (role == null)
                {
                    return NotFound();
                }

                // Eliminar las relaciones con permisos
                role.Permisos.Clear();
                _context.Roles.Remove(role);
                await _context.SaveChangesAsync();
                return Ok();
            }
            catch (DbUpdateException ex)
            {
                // Manejar excepciones específicas de la base de datos
                return StatusCode(500, new { message = "No se puede eliminar el rol porque está siendo referenciado por otras entidades." });
            }
            catch (Exception ex)
            {
                // Manejar otras excepciones
                return StatusCode(500, new { message = "Ocurrió un error al eliminar el rol.", details = ex.Message });
            }
        }

    }
}
