using RefugioVerde.Models;
using RefugioVerde.Servicios.Contrato;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace RefugioVerde.Servicios
{
    public class ClienteServices : IClienteServices
    {
        private readonly RefugioVerdeContext _context;

        public ClienteServices(RefugioVerdeContext context)
        {
            _context = context;
        }

        public async Task<Cliente> SaveCliente(Cliente cliente)
        {
            _context.Clientes.Add(cliente);
            await _context.SaveChangesAsync();
            return cliente;
        }

        public async Task<Cliente> GetClientePorCorreo(string correo)
        {
            return await _context.Clientes.FirstOrDefaultAsync(c => c.Correo == correo);
        }
    }
}
