using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;


namespace RefugioVerde.Models;

public partial class EstadoPago
{
    public int EstadoPagoId { get; set; }
    [Required(ErrorMessage = "El nombre es obligatorio.")]
    [StringLength(20, MinimumLength = 2, ErrorMessage = "El nombre debe tener entre 2 y 20 caracteres.")]
    public string Nombre { get; set; } = null!;
    [JsonIgnore]

    public virtual ICollection<Pago> Pagos { get; set; } = new List<Pago>();
}
