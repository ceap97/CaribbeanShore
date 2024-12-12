namespace TestProject1
{
    internal class Permisos
    {
        public bool TienePermiso(string usuario, string permiso)
        {
            // Lógica de ejemplo
            return usuario == "admin" && permiso == "acceso_total";
        }

        public int ContarPermisos(string usuario)
        {
            // Lógica de ejemplo
            return usuario == "admin" ? 5 : 1;
        }
    }
}
