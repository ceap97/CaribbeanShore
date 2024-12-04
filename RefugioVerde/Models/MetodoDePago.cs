using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace RefugioVerde.Models;

public partial class MetodoDePago
{
    public int MetodoDePagoId { get; set; }

    [Required(ErrorMessage = "El nombre es obligatorio.")]
    [StringLength(50, ErrorMessage = "El nombre no puede tener más de 50 caracteres.")]
    public string Nombre { get; set; } = null!;

    [JsonIgnore]
    public virtual ICollection<Pago> Pagos { get; set; } = new List<Pago>();
}
