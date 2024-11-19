using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RefugioVerde.Models;

namespace RefugioVerde.Controllers
{
    public class HuespedesController : Controller
    {
        private readonly RefugioVerdeContext _context;

        public HuespedesController(RefugioVerdeContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var Huespeds = await _context.Huespeds.Include(h => h.Municipio).Include(h => h.Reserva).ToListAsync();
            return View(Huespeds);
        }

        [HttpGet]
        public async Task<IActionResult> Listar()
        {
            var Huespeds = await _context.Huespeds.Include(h => h.Municipio).Include(h => h.Reserva).ToListAsync();
            return Json(Huespeds);
        }

        // GET: /Huespeds/Obtener/{id}
        [HttpGet]
        public async Task<IActionResult> Obtener(int id)
        {
            var huesped = await _context.Huespeds.Include(h => h.Municipio).Include(h => h.Reserva).FirstOrDefaultAsync(h => h.HuespedId == id);
            if (huesped == null)
            {
                return NotFound();
            }
            return Json(huesped);
        }

        // POST: /Huespeds/Crear
        [HttpPost]
        public async Task<IActionResult> Crear([FromForm] Huesped huesped)
        {
            if (ModelState.IsValid)
            {
                _context.Huespeds.Add(huesped);
                await _context.SaveChangesAsync();
                return Ok();
            }
            return BadRequest(ModelState);
        }

        // POST: /Huespeds/Editar
        [HttpPost]
        public async Task<IActionResult> Editar([FromForm] Huesped huesped)
        {
            if (ModelState.IsValid)
            {
                _context.Huespeds.Update(huesped);
                await _context.SaveChangesAsync();
                return Ok();
            }
            return BadRequest(ModelState);
        }

        // DELETE: /Huespeds/Eliminar/{id}
        [HttpDelete]
        public async Task<IActionResult> Eliminar(int id)
        {
            var huesped = await _context.Huespeds.FindAsync(id);
            if (huesped == null)
            {
                return NotFound();
            }
            _context.Huespeds.Remove(huesped);
            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}
