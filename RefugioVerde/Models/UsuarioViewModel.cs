using System.ComponentModel.DataAnnotations;

namespace RefugioVerde.Models
{
    public class UsuarioViewModel
    {
        public IEnumerable<Usuario> Servicios { get; set; }
        public IEnumerable<Cliente> Comodidades { get; set; }

    }
    }

