using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RefugioVerde.Recursos;

namespace RefugioVerde.Models
{
    public class PermisoService : IPermisoService
    {
        private readonly RefugioVerdeContext _context;

        public PermisoService(RefugioVerdeContext context)
        {
            _context = context;
        }

        public async Task<List<Permiso>> GetPermisosAsync()
        {
            return await _context.Permisos.ToListAsync();
        }
    }
}
