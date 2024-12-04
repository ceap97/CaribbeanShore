using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace RefugioVerde.Models;

public partial class Habitacion
{
    public int HabitacionId { get; set; }

    [Required(ErrorMessage = "El número de habitación es obligatorio.")]
    [StringLength(4, ErrorMessage = "El número de habitación no puede tener más de 4 caracteres.")]
    public string Numero { get; set; } = null!;

    [Required(ErrorMessage = "El nombre de la habitación es obligatorio.")]
    [StringLength(50, ErrorMessage = "El nombre de la habitación no puede tener más de 50 caracteres.")]
    public string NombreHabitacion { get; set; } = null!;

    [Required(ErrorMessage = "El tipo de habitación es obligatorio.")]
    [StringLength(20, ErrorMessage = "El tipo de habitación no puede tener más de 20 caracteres.")]
    public string Tipo { get; set; } = null!;

    [Required(ErrorMessage = "El precio es obligatorio.")]
    [Range(0, double.MaxValue, ErrorMessage = "El precio debe ser un valor positivo.")]
    public decimal Precio { get; set; }

    [Required(ErrorMessage = "El estado de la habitación es obligatorio.")]
    public int EstadoHabitacionId { get; set; }

    [Url(ErrorMessage = "La imagen debe ser una URL válida.")]
    public string Imagen { get; set; }

    [Range(1, 5, ErrorMessage = "La capacidad debe estar entre 1 y 5.")]
    public int Capacidad { get; set; }

    [JsonIgnore]
    public virtual EstadoHabitacion EstadoHabitacion { get; set; } = null!;

    [JsonIgnore]
    public virtual ICollection<Reserva> Reservas { get; set; } = new List<Reserva>();
    public virtual ICollection<EncuestaSatisfaccion> EncuestasSatisfaccion { get; set; }
}
