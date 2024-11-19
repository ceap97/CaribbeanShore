using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RefugioVerde.Models;

namespace RefugioVerde.Controllers
{
    public class EmpleadosController : Controller
    {
        private readonly RefugioVerdeContext _context;

        public EmpleadosController(RefugioVerdeContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var empleados = await _context.Empleados.Include(e => e.Municipio).Include(e => e.Rol).ToListAsync();
            return View(empleados);
        }

        [HttpGet]
        public async Task<IActionResult> Listar()
        {
            var empleados = await _context.Empleados.Include(e => e.Municipio).Include(e => e.Rol).ToListAsync();
            return Json(empleados);
        }

        // GET: /Empleados/Obtener/{id}
        [HttpGet]
        public async Task<IActionResult> Obtener(int id)
        {
            var empleado = await _context.Empleados.Include(e => e.Municipio).Include(e => e.Rol).FirstOrDefaultAsync(e => e.EmpleadoId == id);
            if (empleado == null)
            {
                return NotFound();
            }
            return Json(empleado);
        }

        // POST: /Empleados/Crear
        [HttpPost]
        public async Task<IActionResult> Crear([FromForm] Empleado empleado)
        {
            if (ModelState.IsValid)
            {
                _context.Empleados.Add(empleado);
                await _context.SaveChangesAsync();
                return Ok();
            }
            return BadRequest(ModelState);
        }

        // POST: /Empleados/Editar
        [HttpPost]
        public async Task<IActionResult> Editar([FromForm] Empleado empleado)
        {
            if (ModelState.IsValid)
            {
                _context.Empleados.Update(empleado);
                await _context.SaveChangesAsync();
                return Ok();
            }
            return BadRequest(ModelState);
        }

        // DELETE: /Empleados/Eliminar/{id}
        [HttpDelete]
        public async Task<IActionResult> Eliminar(int id)
        {
            var empleado = await _context.Empleados.FindAsync(id);
            if (empleado == null)
            {
                return NotFound();
            }
            _context.Empleados.Remove(empleado);
            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}
