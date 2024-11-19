using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace RefugioVerde.Models;

public partial class EstadoHabitacion
{
    public int EstadoHabitacionId { get; set; }
    [Required(ErrorMessage = "El nombre es obligatorio.")]
    [StringLength(20, MinimumLength = 2, ErrorMessage = "El nombre debe tener entre 2 y 20 caracteres.")]
    public string Nombre { get; set; } = null!;
    [JsonIgnore]

    public virtual ICollection<Habitacion> Habitacions { get; set; } = new List<Habitacion>();
}
