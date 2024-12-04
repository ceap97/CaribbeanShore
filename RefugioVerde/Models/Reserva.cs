using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace RefugioVerde.Models
{
    public partial class Reserva
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

        [JsonIgnore]
        public virtual Cliente Cliente { get; set; } = null!;
        [JsonIgnore]
        public virtual EstadoReserva EstadoReserva { get; set; } = null!;
        [JsonIgnore]
        public virtual Habitacion Habitacion { get; set; } = null!;
        [JsonIgnore]
        public virtual ICollection<Huesped> Huespeds { get; set; } = new List<Huesped>();
        [JsonIgnore]
        public virtual ICollection<Pago> Pagos { get; set; } = new List<Pago>();
        [JsonIgnore]
        public virtual ICollection<Comodidad> Comodidades { get; set; } = new List<Comodidad>();
        [JsonIgnore]
        public virtual ICollection<Servicio> Servicios { get; set; } = new List<Servicio>();
    }
}
