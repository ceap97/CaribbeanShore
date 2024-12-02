using System.Collections.Generic;
using System.Threading.Tasks;

namespace RefugioVerde.Models
{
    public interface IRoleService
    {
        Task<List<Role>> GetRolesAsync();
    }
}
