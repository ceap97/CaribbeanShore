using Microsoft.EntityFrameworkCore;
using RefugioVerde;
using RefugioVerde.Models;



namespace RefugioVerde.Servicios.Contrato
{
    public interface IUsuarioServices
    {
        Task<Usuario> GetUsuario(string correo, string clave);
        Task<Usuario> SaveUsuario(Usuario modelo);  
    }
}
