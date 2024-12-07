using RefugioVerde.Models;
using System.Threading.Tasks;

namespace RefugioVerde.Servicios.Contrato
{
    public interface IClienteServices
    {
        Task<Cliente> SaveCliente(Cliente cliente);
        Task<Cliente> GetClientePorCorreo(string correo);
    }
}
