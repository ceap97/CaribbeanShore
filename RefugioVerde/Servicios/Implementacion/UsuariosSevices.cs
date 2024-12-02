using Microsoft.EntityFrameworkCore;
using RefugioVerde.Models;
using RefugioVerde.Servicios.Contrato;

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

    public async Task<Usuario> GetUsuarioPorCorreo(string correo)
    {
        return await _dbContext.Usuarios.FirstOrDefaultAsync(u => u.Correo == correo);
    }


    public async Task<Usuario> SaveUsuario(Usuario modelo)
    {
        // Verificar si el correo ya está registrado
        var usuarioExistente = await _dbContext.Usuarios.FirstOrDefaultAsync(u => u.Correo == modelo.Correo);
        if (usuarioExistente != null)
        {
            throw new Exception("El correo ya está registrado.");
        }

        _dbContext.Usuarios.Add(modelo);
        await _dbContext.SaveChangesAsync();
        return modelo;
    }
}
