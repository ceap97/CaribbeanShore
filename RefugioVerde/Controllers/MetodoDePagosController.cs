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
    public class MetodoDePagosController : Controller
    {
        private readonly RefugioVerdeContext _context;

        public MetodoDePagosController(RefugioVerdeContext context)
        {
            _context = context;
        }

        // GET: MetodoDePagos
        public async Task<IActionResult> Index()
        {
            return View(await _context.MetodoDePagos.ToListAsync());
        }

        // GET: MetodoDePagos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var metodoDePago = await _context.MetodoDePagos
                .FirstOrDefaultAsync(m => m.MetodoDePagoId == id);
            if (metodoDePago == null)
            {
                return NotFound();
            }

            return View(metodoDePago);
        }

        // GET: MetodoDePagos/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: MetodoDePagos/Create
        [HttpPost]
        public async Task<IActionResult> Create([Bind("MetodoDePagoId,Nombre")] MetodoDePago metodoDePago)
        {
            if (ModelState.IsValid)
            {
                _context.Add(metodoDePago);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(metodoDePago);
        }

        // GET: MetodoDePagos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var metodoDePago = await _context.MetodoDePagos.FindAsync(id);
            if (metodoDePago == null)
            {
                return NotFound();
            }
            return View(metodoDePago);
        }

        // POST: MetodoDePagos/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MetodoDePagoId,Nombre")] MetodoDePago metodoDePago)
        {
            if (id != metodoDePago.MetodoDePagoId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(metodoDePago);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MetodoDePagoExists(metodoDePago.MetodoDePagoId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(metodoDePago);
        }

        // GET: MetodoDePagos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var metodoDePago = await _context.MetodoDePagos
                .FirstOrDefaultAsync(m => m.MetodoDePagoId == id);
            if (metodoDePago == null)
            {
                return NotFound();
            }

            return View(metodoDePago);
        }

        // POST: MetodoDePagos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var metodoDePago = await _context.MetodoDePagos.FindAsync(id);
            if (metodoDePago != null)
            {
                _context.MetodoDePagos.Remove(metodoDePago);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MetodoDePagoExists(int id)
        {
            return _context.MetodoDePagos.Any(e => e.MetodoDePagoId == id);
        }

        // GET: MetodoDePagos/Listar
        [HttpGet]
        public async Task<IActionResult> Listar()
        {
            var metodoDePagos = await _context.MetodoDePagos.ToListAsync();
            return Json(metodoDePagos);
        }

        // GET: MetodoDePagos/Obtener/5
        [HttpGet]
        public async Task<IActionResult> Obtener(int id)
        {
            var metodoDePago = await _context.MetodoDePagos.FindAsync(id);
            if (metodoDePago == null)
            {
                return NotFound();
            }
            return Json(metodoDePago);
        }

        // POST: MetodoDePagos/CrearModal
        [HttpPost]
        public async Task<IActionResult> CrearModal([FromForm] MetodoDePago metodoDePago)
        {
            if (ModelState.IsValid)
            {
                _context.MetodoDePagos.Add(metodoDePago);
                await _context.SaveChangesAsync();
                return Ok();
            }
            return BadRequest(ModelState);
        }

        // POST: MetodoDePagos/EditarModal
        [HttpPost]
        public async Task<IActionResult> EditarModal([FromForm] MetodoDePago metodoDePago)
        {
            if (ModelState.IsValid)
            {
                _context.MetodoDePagos.Update(metodoDePago);
                await _context.SaveChangesAsync();
                return Ok();
            }
            return BadRequest(ModelState);
        }

        // DELETE: MetodoDePagos/EliminarModal/5
        [HttpDelete]
        public async Task<IActionResult> EliminarModal(int id)
        {
            var metodoDePago = await _context.MetodoDePagos.FindAsync(id);
            if (metodoDePago == null)
            {
                return NotFound();
            }
            _context.MetodoDePagos.Remove(metodoDePago);
            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}
