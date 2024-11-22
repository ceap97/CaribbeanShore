using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RefugioVerde.Models;

namespace RefugioVerde.Controllers
{
   
    public class ClientesController : Controller
    {
        private readonly RefugioVerdeContext _context;

        public ClientesController(RefugioVerdeContext context)
        {
            _context = context;
        }
        [HttpGet]
        public IActionResult MiPerfil()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (int.TryParse(userId, out int parsedUserId))
            {
                var cliente = _context.Clientes.Include(c => c.Usuario).Include(m => m.Municipio).FirstOrDefault(c => c.UsuarioId == parsedUserId);
                if (cliente == null)
                {
                    return NotFound();
                }
                return View(cliente);
            }
            return BadRequest("Invalid user ID");
        }


        [HttpPost]
        public IActionResult ActualizarPerfil(Cliente cliente)
        {
            if (ModelState.IsValid)
            {
                var clienteDb = _context.Clientes.Include(c => c.Usuario).FirstOrDefault(c => c.ClienteId == cliente.ClienteId);
                if (clienteDb != null)
                {
                    clienteDb.Nombre = cliente.Nombre;
                    clienteDb.Apellido = cliente.Apellido;
                    clienteDb.DocumentoIdentidad = cliente.DocumentoIdentidad;
                    clienteDb.Telefono = cliente.Telefono;
                    clienteDb.Correo = cliente.Correo;
                    clienteDb.Usuario.NombreUsuario = cliente.Usuario.NombreUsuario;
                    clienteDb.Usuario.Clave = cliente.Usuario.Clave;

                    _context.SaveChanges();
                    return RedirectToAction("MiPerfil");
                }
            }
            return View("MiPerfil", cliente);
        }
        public IActionResult Create()
        {
            ViewBag.MunicipioId = new SelectList(_context.Municipios, "Id", "Nombre");
            ViewBag.UsuarioId = new SelectList(_context.Usuarios, "Id", "NombreUsuario");
            return PartialView("_Create");
        }

        // POST: Clientes/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
       
        public async Task<IActionResult> Create([Bind("Nombre,Apellido,DocumentoIdentidad,MunicipioId,Telefono,Correo,UsuarioId")] Cliente cliente)
        {
            if (ModelState.IsValid)
            {
                _context.Add(cliente);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewBag.MunicipioId = new SelectList(_context.Municipios, "Id", "Nombre", cliente.MunicipioId);
            ViewBag.UsuarioId = new SelectList(_context.Usuarios, "Id", "NombreUsuario", cliente.UsuarioId);
            return PartialView("_Create", cliente);
        }
        public async Task<IActionResult> Index()
        {
            var clientes = await _context.Clientes.Include(c => c.Municipio).ToListAsync();
            return View(clientes);
        }

        [HttpGet]
        public async Task<IActionResult> Listar()
        {
            var clientes = await _context.Clientes.Include(c => c.Municipio).ToListAsync();
            return Json(clientes);
        }

        // GET: /Clientes/Obtener/{id}
        [HttpGet]
        public async Task<IActionResult> Obtener(int id)
        {
            var cliente = await _context.Clientes.Include(c => c.Municipio).FirstOrDefaultAsync(c => c.ClienteId == id);
            if (cliente == null)
            {
                return NotFound();
            }
            return Json(cliente);
        }

        // POST: /Clientes/Crear
        [HttpPost]
        public async Task<IActionResult> Crear([FromForm] Cliente cliente)
        {
            if (ModelState.IsValid)
            {
                _context.Clientes.Add(cliente);
                await _context.SaveChangesAsync();
                return Ok();
            }
            return BadRequest(ModelState);
        }

        // POST: /Clientes/Editar
        [HttpPost]
        public async Task<IActionResult> Editar([FromForm] Cliente cliente)
        {
            if (ModelState.IsValid)
            {
                _context.Clientes.Update(cliente);
                await _context.SaveChangesAsync();
                return Ok();
            }
            return BadRequest(ModelState);
        }

        // DELETE: /Clientes/Eliminar/{id}
        [HttpDelete]
        public async Task<IActionResult> Eliminar(int id)
        {
            var cliente = await _context.Clientes.FindAsync(id);
            if (cliente == null)
            {
                return NotFound();
            }
            _context.Clientes.Remove(cliente);
            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}
