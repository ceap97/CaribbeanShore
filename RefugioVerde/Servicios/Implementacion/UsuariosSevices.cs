using Microsoft.EntityFrameworkCore;
using RefugioVerde.Servicios.Contrato;
using RefugioVerde.Models;

namespace RefugioVerde.Servicios.Implementacion
{
    public class UsuariosSevices : IUsuarioServices
    {

        private readonly RefugioVerdeContext _dbContext;
        public UsuariosSevices(RefugioVerdeContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<Usuario> GetUsuario(string correo, string clave)
        {
            Usuario usuario_encontrado = await _dbContext.Usuarios.Where(u => u.Correo == correo && u.Clave == clave)
                .FirstOrDefaultAsync();

            return usuario_encontrado;
        }

        public async Task<Usuario> SaveUsuario(Usuario modelo)
        {
            _dbContext.Usuarios.Add(modelo);
            await _dbContext.SaveChangesAsync();
            return modelo;
        }
    }
}
