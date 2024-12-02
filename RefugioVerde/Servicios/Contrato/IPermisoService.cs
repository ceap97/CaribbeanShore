using System.Collections.Generic;
using System.Threading.Tasks;

namespace RefugioVerde.Models
{
    public interface IPermisoService
    {
        Task<List<Permiso>> GetPermisosAsync();
    }
}
