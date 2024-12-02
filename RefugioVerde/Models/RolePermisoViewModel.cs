using System.Collections.Generic;

namespace RefugioVerde.Models
{
    public class RolePermisoViewModel
    {
        public int RolId { get; set; }
        public string NombreRol { get; set; }
        public List<PermisoViewModel> Permisos { get; set; } = new List<PermisoViewModel>();
    }

    public class PermisoViewModel
    {
        public int PermisoId { get; set; }
        public string NombrePermiso { get; set; }
        public bool Asignado { get; set; }
    }
}
