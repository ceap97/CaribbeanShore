using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RefugioVerde.Models;

namespace RefugioVerde.Controllers
{
    public class MunicipiosController : Controller
    {
        private readonly RefugioVerdeContext _context;

        public MunicipiosController(RefugioVerdeContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var municipios = await _context.Municipios.Include(m => m.Departamento).ToListAsync();
            return View(municipios);
        }

        [HttpGet]
        public async Task<IActionResult> Listar()
        {
            var municipios = await _context.Municipios.Include(m => m.Departamento).ToListAsync();
            return Json(municipios);
        }

        // GET: /Municipios/Obtener/{id}
        [HttpGet]
        public async Task<IActionResult> Obtener(int id)
        {
            var municipio = await _context.Municipios.Include(m => m.Departamento).FirstOrDefaultAsync(m => m.MunicipioId == id);
            if (municipio == null)
            {
                return NotFound();
            }
            return Json(municipio);
        }

        // POST: /Municipios/Crear
        [HttpPost]
        public async Task<IActionResult> Crear([FromForm] Municipio municipio)
        {
            if (ModelState.IsValid)
            {
                _context.Municipios.Add(municipio);
                await _context.SaveChangesAsync();
                return Ok();
            }
            return BadRequest(ModelState);
        }

        // POST: /Municipios/Editar
        [HttpPost]
        public async Task<IActionResult> Editar([FromForm] Municipio municipio)
        {
            if (ModelState.IsValid)
            {
                _context.Municipios.Update(municipio);
                await _context.SaveChangesAsync();
                return Ok();
            }
            return BadRequest(ModelState);
        }

        // DELETE: /Municipios/Eliminar/{id}
        [HttpDelete]
        public async Task<IActionResult> Eliminar(int id)
        {
            var municipio = await _context.Municipios.FindAsync(id);
            if (municipio == null)
            {
                return NotFound();
            }
            _context.Municipios.Remove(municipio);
            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}
