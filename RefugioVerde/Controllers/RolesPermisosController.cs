using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RefugioVerde.Models;
using System.Linq;
using System.Threading.Tasks;

namespace RefugioVerde.Controllers
{
    public class RolesPermisosController : Controller
    {
        private readonly RefugioVerdeContext _context;

        public RolesPermisosController(RefugioVerdeContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var roles = await _context.Roles
                .Include(r => r.Permisos)
                .Select(r => new RolePermisoViewModel
                {
                    RolId = r.RolId,
                    NombreRol = r.Nombre,
                    Permisos = r.Permisos.Select(p => new PermisoViewModel
                    {
                        PermisoId = p.PermisoId,
                        NombrePermiso = p.Nombre,
                        Asignado = true
                    }).ToList()
                }).ToListAsync();

            var permisos = await _context.Permisos.ToListAsync();

            foreach (var role in roles)
            {
                foreach (var permiso in permisos)
                {
                    if (!role.Permisos.Any(p => p.PermisoId == permiso.PermisoId))
                    {
                        role.Permisos.Add(new PermisoViewModel
                        {
                            PermisoId = permiso.PermisoId,
                            NombrePermiso = permiso.Nombre,
                            Asignado = false
                        });
                    }
                }
            }

            return View(roles);
        }

        [HttpPost]
        public async Task<IActionResult> Guardar(List<RolePermisoViewModel> model)
        {
            foreach (var roleModel in model)
            {
                var rol = await _context.Roles
                    .Include(r => r.Permisos)
                    .FirstOrDefaultAsync(r => r.RolId == roleModel.RolId);

                if (rol != null)
                {
                    // Obtener los permisos actuales del rol
                    var permisosActuales = rol.Permisos.ToList();

                    // Eliminar los permisos que ya no están asignados
                    foreach (var permiso in permisosActuales)
                    {
                        if (!roleModel.Permisos.Any(p => p.PermisoId == permiso.PermisoId && p.Asignado))
                        {
                            rol.Permisos.Remove(permiso);
                        }
                    }

                    // Agregar los nuevos permisos asignados
                    foreach (var permiso in roleModel.Permisos.Where(p => p.Asignado))
                    {
                        if (!rol.Permisos.Any(p => p.PermisoId == permiso.PermisoId))
                        {
                            var permisoEntity = await _context.Permisos.FindAsync(permiso.PermisoId);
                            if (permisoEntity != null)
                            {
                                rol.Permisos.Add(permisoEntity);
                            }
                        }
                    }
                }
            }

            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }


    }
}
