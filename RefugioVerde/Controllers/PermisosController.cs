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
    public class PermisosController : Controller
    {
        private readonly RefugioVerdeContext _context;

        public PermisosController(RefugioVerdeContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var permisos = await _context.Permisos.ToListAsync();
            return View(permisos);
        }

        [HttpGet]
        public async Task<IActionResult> Listar()
        {
            var permisos = await _context.Permisos.ToListAsync();
            return Json(permisos);
        }

        // GET: /Permisos/Obtener/{id}
        [HttpGet]
        public async Task<IActionResult> Obtener(int id)
        {
            var permiso = await _context.Permisos.FindAsync(id);
            if (permiso == null)
            {
                return NotFound();
            }
            return Json(permiso);
        }

        // POST: /Permisos/Crear
        [HttpPost]
        public async Task<IActionResult> Crear([FromForm] Permiso permiso)
        {
            if (ModelState.IsValid)
            {
                _context.Permisos.Add(permiso);
                await _context.SaveChangesAsync();
                return Ok();
            }
            return BadRequest(ModelState);
        }

        // POST: /Permisos/Editar
        [HttpPost]
        public async Task<IActionResult> Editar([FromForm] Permiso permiso)
        {
            if (ModelState.IsValid)
            {
                _context.Permisos.Update(permiso);
                await _context.SaveChangesAsync();
                return Ok();
            }
            return BadRequest(ModelState);
        }

        // DELETE: /Permisos/Eliminar/{id}
        [HttpDelete]
        public async Task<IActionResult> Eliminar(int id)
        {
            var permiso = await _context.Permisos.FindAsync(id);
            if (permiso == null)
            {
                return NotFound();
            }
            _context.Permisos.Remove(permiso);
            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}