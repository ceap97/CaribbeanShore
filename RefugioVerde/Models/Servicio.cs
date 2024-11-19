using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace RefugioVerde.Models;

public partial class Servicio
{
    public int ServicioId { get; set; }

    [Required(ErrorMessage = "El nombre es obligatorio.")]
    [StringLength(30, MinimumLength = 4, ErrorMessage = "El nombre debe tener entre 4 y 30 caracteres.")]
    [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "El nombre solo puede contener letras y espacios.")]
    public string Nombre { get; set; } = null!;

    [Required(ErrorMessage = "La descripción es obligatoria.")]
    [StringLength(200, ErrorMessage = "La descripción no puede tener más de 200 caracteres.")]
    public string Descripcion { get; set; } = null!;

    //[Required(ErrorMessage = "La imagen es obligatoria.")]
    public byte[] Imagen { get; set; } = null!;

    [Required(ErrorMessage = "El precio es obligatorio.")]
    [Range(0.01, double.MaxValue, ErrorMessage = "El precio debe ser un valor positivo.")]
    public decimal Precio { get; set; }

    [JsonIgnore]
    public virtual ICollection<Reserva> Reservas { get; set; } = new List<Reserva>();
}
