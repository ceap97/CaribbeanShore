using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Text.Json.Serialization;

namespace RefugioVerde.Models
{
    public partial class Comodidad
    {
        public int ComodidadId { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio.")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "El nombre debe tener entre 2 y 50 caracteres.")]
        public string Nombre { get; set; } = null!;
        [Required(ErrorMessage = "El descripción es obligatorio.")]
        [StringLength(200, ErrorMessage = "La descripción no puede tener más de 200 caracteres.")]
        public string Descripcion { get; set; }
        [Required(ErrorMessage = "El precio es obligatorio.")]
        [Range(0, double.MaxValue, ErrorMessage = "El precio debe ser un valor positivo.")]
        public decimal Precio { get; set; }

        [Url(ErrorMessage = "La imagen debe ser una URL válida.")]
        public string Imagen { get; set; }

        [JsonIgnore]
        public virtual ICollection<Reserva> Reservas { get; set; } = new List<Reserva>();
        [JsonIgnore]
        public string PrecioFormateado => Precio.ToString("N0", new CultureInfo("es-ES"));
    }
}
