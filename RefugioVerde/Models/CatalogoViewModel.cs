using RefugioVerde.Models;

public class CatalogoViewModel
{
    public IEnumerable<Habitacion> Habitaciones { get; set; }
    public bool UsuarioAsociadoConCliente { get; set; }
}
