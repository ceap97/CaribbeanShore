using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RefugioVerde.Models;

namespace RefugioVerde.Controllers
{
    public class DepartamentosController : Controller
    {
        private readonly RefugioVerdeContext _context;

        public DepartamentosController(RefugioVerdeContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var departamentos = await _context.Departamentos.ToListAsync();
            return View(departamentos);
        }

        [HttpGet]
        public async Task<IActionResult> Listar()
        {
            var departamentos = await _context.Departamentos.ToListAsync();
            return Json(departamentos);
        }

        // GET: /Departamentos/Obtener/{id}
        [HttpGet]
        public async Task<IActionResult> Obtener(int id)
        {
            var departamento = await _context.Departamentos.FindAsync(id);
            if (departamento == null)
            {
                return NotFound();
            }
            return Json(departamento);
        }

        // POST: /Departamentos/Crear
        [HttpPost]
        public async Task<IActionResult> Crear([FromForm] Departamento departamento)
        {
            if (ModelState.IsValid)
            {
                _context.Departamentos.Add(departamento);
                await _context.SaveChangesAsync();
                return Ok();
            }
            return BadRequest(ModelState);
        }

        // POST: /Departamentos/Editar
        [HttpPost]
        public async Task<IActionResult> Editar([FromForm] Departamento departamento)
        {
            if (ModelState.IsValid)
            {
                _context.Departamentos.Update(departamento);
                await _context.SaveChangesAsync();
                return Ok();
            }
            return BadRequest(ModelState);
        }

        // DELETE: /Departamentos/Eliminar/{id}
        [HttpDelete]
        public async Task<IActionResult> Eliminar(int id)
        {
            var departamento = await _context.Departamentos.FindAsync(id);
            if (departamento == null)
            {
                return NotFound();
            }
            _context.Departamentos.Remove(departamento);
            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}
