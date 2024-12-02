using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RefugioVerde.Models;

namespace RefugioVerde.Controllers
{
    public class EstadoReservasController : Controller
    {
        private readonly RefugioVerdeContext _context;

        public EstadoReservasController(RefugioVerdeContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var estadosReserva = await _context.EstadoReservas.ToListAsync();
            return View(estadosReserva);
        }

        [HttpGet]
        public async Task<IActionResult> Listar()
        {
            var estadosReserva = await _context.EstadoReservas.ToListAsync();
            return Json(estadosReserva);
        }

        // GET: /EstadoReservas/Obtener/{id}
        [HttpGet]
        public async Task<IActionResult> Obtener(int id)
        {
            var estadoReserva = await _context.EstadoReservas.FindAsync(id);
            if (estadoReserva == null)
            {
                return NotFound();
            }
            return Json(estadoReserva);
        }

        // POST: /EstadoReservas/Crear
        // POST: /EstadoReservas/Crear
        [HttpPost]
        public async Task<IActionResult> Crear([FromForm] EstadoReserva estadoReserva)
        {
            if (ModelState.IsValid)
            {
                _context.EstadoReservas.Add(estadoReserva);
                await _context.SaveChangesAsync();
                return Ok();
            }
            return BadRequest(ModelState);
        }

        // POST: /EstadoReservas/Editar
        [HttpPost]
        public async Task<IActionResult> Editar([FromForm] EstadoReserva estadoReserva)
        {
            if (ModelState.IsValid)
            {
                _context.EstadoReservas.Update(estadoReserva);
                await _context.SaveChangesAsync();
                return Ok();
            }
            return BadRequest(ModelState);
        }


        // DELETE: /EstadoReservas/Eliminar/{id}
        [HttpDelete]
        public async Task<IActionResult> Eliminar(int id)
        {
            var estadoReserva = await _context.EstadoReservas.FindAsync(id);
            if (estadoReserva == null)
            {
                return NotFound();
            }
            _context.EstadoReservas.Remove(estadoReserva);
            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}
