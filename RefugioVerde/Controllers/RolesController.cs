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
                return View(roles);
            }

            [HttpGet]
            public async Task<IActionResult> Listar()
            {
                var roles = await _context.Roles.ToListAsync();
                return Json(roles);
            }

            // GET: /Roles/Obtener/{id}
            [HttpGet]
            public async Task<IActionResult> Obtener(int id)
            {
                var role = await _context.Roles.FirstOrDefaultAsync(r => r.RolId == id);
                if (role == null)
                {
                    return NotFound();
                }
                return Json(role);
            }

            // GET: /Roles/Detalles/{id}
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

            // POST: /Roles/Crear
            [HttpPost]
            public async Task<IActionResult> Crear([FromForm] Role role)
            {
                if (ModelState.IsValid)
                {
                    _context.Roles.Add(role);
                    await _context.SaveChangesAsync();
                    return Ok();
                }
                return BadRequest(ModelState);
            }

            // POST: /Roles/Editar
            [HttpPost]
            public async Task<IActionResult> Editar([FromForm] Role role)
            {
                if (ModelState.IsValid)
                {
                    _context.Roles.Update(role);
                    await _context.SaveChangesAsync();
                    return Ok();
                }
                return BadRequest(ModelState);
            }

            // DELETE: /Roles/Eliminar/{id}
            [HttpDelete]
            public async Task<IActionResult> Eliminar(int id)
            {
                var role = await _context.Roles.FindAsync(id);
                if (role == null)
                {
                    return NotFound();
                }
                _context.Roles.Remove(role);
                await _context.SaveChangesAsync();
                return Ok();
            }
        }
    }
