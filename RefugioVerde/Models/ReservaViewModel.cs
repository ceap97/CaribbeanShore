namespace RefugioVerde.Models
{
    public class ReservaViewModel
    {
        public int ReservaId { get; set; }
        public int? ClienteId { get; set; }
        public DateTime FechaReserva { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public string Habitacion { get; set; }
        public string EstadoReserva { get; set; }
        public string Servicio { get; set; }
        public string Comodidad { get; set; }

    }
}
