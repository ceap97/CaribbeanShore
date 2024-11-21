using RefugioVerde.Models;

public class HomeViewModel
{
    public List<Habitacion> Habitaciones { get; set; } = new List<Habitacion>();
    public IEnumerable<Servicio> Servicios { get; set; }
    public IEnumerable<Comodidad> Comodidades { get; set; }
}
