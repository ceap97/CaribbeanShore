namespace TestProject1
{
    internal class Permisos
    {
        public bool TienePermiso(string usuario, string permiso)
        {
            // L�gica de ejemplo
            return usuario == "admin" && permiso == "acceso_total";
        }

        public int ContarPermisos(string usuario)
        {
            // L�gica de ejemplo
            return usuario == "admin" ? 5 : 1;
        }
    }
}
