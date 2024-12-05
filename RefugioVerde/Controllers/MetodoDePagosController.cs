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
            return View(await _context.MetodoDePago.ToListAsync());
        }

        // GET: MetodoDePagos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var metodoDePago = await _context.MetodoDePago
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
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
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

            var metodoDePago = await _context.MetodoDePago.FindAsync(id);
            if (metodoDePago == null)
            {
                return NotFound();
            }
            return View(metodoDePago);
        }

        // POST: MetodoDePagos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
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

            var metodoDePago = await _context.MetodoDePago
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
            var metodoDePago = await _context.MetodoDePago.FindAsync(id);
            if (metodoDePago != null)
            {
                _context.MetodoDePago.Remove(metodoDePago);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MetodoDePagoExists(int id)
        {
            return _context.MetodoDePago.Any(e => e.MetodoDePagoId == id);
        }
    }
}
