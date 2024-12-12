using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace RefugioVerde.Models
{
    public class ReservaViewModel : IValidatableObject
    {
        public int ReservaId { get; set; }
        public DateTime FechaReserva { get; set; } = DateTime.Now;
        public int? ClienteId { get; set; }
        public int? HabitacionId { get; set; }
        public int? EstadoReservaId { get; set; } = 2;
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public decimal MontoTotal { get; set; }
        public string Confirmacion { get; set; }

        public string Habitacion { get; set; }
        public string EstadoReserva { get; set; }
        public string Servicio { get; set; }
        public string Comodidad { get; set; }
        public decimal PrecioHabitacion { get; set; }
        public decimal PrecioComodidad { get; set; }
        public decimal PrecioServicio { get; set; }

        public List<string> Servicios { get; internal set; }
        public List<string> Comodidades { get; internal set; }

        
        public string MontoTotalFormateado => MontoTotal.ToString("N0", new CultureInfo("es-ES"));

        public string PrecioHabitacionFormateado => PrecioHabitacion.ToString("N0", new CultureInfo("es-ES"));

        public string PrecioComodidadFormateado => PrecioComodidad.ToString("N0", new CultureInfo("es-ES"));

        
        public string PrecioServicioFormateado => PrecioServicio.ToString("N0", new CultureInfo("es-ES"));

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (FechaFin <= FechaInicio)
            {
                yield return new ValidationResult("La fecha de fin debe ser mayor que la fecha de inicio.", new[] { nameof(FechaFin) });
            }
        }
    }
}
