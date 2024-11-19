using RefugioVerde.Models;

public class HomeViewModel
{
    public IEnumerable<Habitacion> Habitaciones { get; set; }
    public IEnumerable<Servicio> Servicios { get; set; }
    public IEnumerable<Comodidad> Comodidades { get; set; }
}
