using System.ComponentModel.DataAnnotations;

namespace RefugioVerde.Models
{
    public class ReservaViewModel : IValidatableObject
    {
        public int ReservaId { get; set; }
        public DateTime FechaReserva { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public string Habitacion { get; set; }
        public string EstadoReserva { get; set; }
        public string Servicio { get; set; }
        public string Comodidad { get; set; }
        public decimal PrecioHabitacion { get; set; }
        public decimal PrecioComodidad { get; set; }
        public decimal PrecioServicio { get; set; }
        public decimal MontoTotal => (PrecioHabitacion + PrecioComodidad + PrecioServicio) * ((FechaFin - FechaInicio).Days == 0 ? 1 : (FechaFin - FechaInicio).Days);

        public List<string> Servicios { get; internal set; }
        public List<string> Comodidades { get; internal set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (FechaFin <= FechaInicio)
            {
                yield return new ValidationResult("La fecha de fin debe ser mayor que la fecha de inicio.", new[] { nameof(FechaFin) });
            }
        }
    }
}
