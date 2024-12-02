using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace RefugioVerde.Models
{
    public class RoleService : IRoleService
    {
        private readonly RefugioVerdeContext _context;

        public RoleService(RefugioVerdeContext context)
        {
            _context = context;
        }

        public async Task<List<Role>> GetRolesAsync()
        {
            return await _context.Roles.Include(r => r.Permisos).ToListAsync();
        }
    }
}
