using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RefugioVerde.Models;

namespace RefugioVerde.Controllers
{
    public class EstadoHabitacionesController : Controller
    {
        private readonly RefugioVerdeContext _context;

        public EstadoHabitacionesController(RefugioVerdeContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var estadosHabitacion = await _context.EstadoHabitacions.ToListAsync();
            return View(estadosHabitacion);
        }

        [HttpGet]
        public async Task<IActionResult> Listar()
        {
            var estadosHabitacion = await _context.EstadoHabitacions.ToListAsync();
            return Json(estadosHabitacion);
        }

        // GET: /EstadoHabitaciones/Obtener/{id}
        [HttpGet]
        public async Task<IActionResult> Obtener(int id)
        {
            var estadoHabitacion = await _context.EstadoHabitacions.FindAsync(id);
            if (estadoHabitacion == null)
            {
                return NotFound();
            }
            return Json(estadoHabitacion);
        }

        // POST: /EstadoHabitaciones/Crear
        [HttpPost]
        public async Task<IActionResult> Crear([FromForm] EstadoHabitacion estadoHabitacion)
        {
            if (ModelState.IsValid)
            {
                _context.EstadoHabitacions.Add(estadoHabitacion);
                await _context.SaveChangesAsync();
                return Ok();
            }
            return BadRequest(ModelState);
        }

        // POST: /EstadoHabitaciones/Editar
        [HttpPost]
        public async Task<IActionResult> Editar([FromForm] EstadoHabitacion estadoHabitacion)
        {
            if (ModelState.IsValid)
            {
                _context.EstadoHabitacions.Update(estadoHabitacion);
                await _context.SaveChangesAsync();
                return Ok();
            }
            return BadRequest(ModelState);
        }

        // DELETE: /EstadoHabitaciones/Eliminar/{id}
        [HttpDelete]
        public async Task<IActionResult> Eliminar(int id)
        {
            var estadoHabitacion = await _context.EstadoHabitacions.FindAsync(id);
            if (estadoHabitacion == null)
            {
                return NotFound();
            }
            _context.EstadoHabitacions.Remove(estadoHabitacion);
            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}
