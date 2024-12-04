using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace RefugioVerde.Models;

public partial class Cliente
{
    public int ClienteId { get; set; }

    [Required(ErrorMessage = "El nombre es obligatorio.")]
    [StringLength(20, MinimumLength = 2, ErrorMessage = "El nombre debe tener entre 2 y 20 caracteres.")]
    public string Nombre { get; set; } = null!;

    [Required(ErrorMessage = "El apellido es obligatorio.")]
    [StringLength(20, MinimumLength = 2, ErrorMessage = "El apellido debe tener entre 2 y 20 caracteres.")]
    public string Apellido { get; set; } = null!;

    [Required(ErrorMessage = "El documento de identidad es obligatorio.")]
    [StringLength(18, MinimumLength = 8, ErrorMessage = "El documento de identidad debe tener entre 8 y 18 caracteres.")]
    public string DocumentoIdentidad { get; set; } = null!;

    [Required(ErrorMessage = "El teléfono es obligatorio.")]
    [Phone(ErrorMessage = "El teléfono no tiene un formato válido.")]
    public string Telefono { get; set; }

    [Required(ErrorMessage = "El correo es obligatorio.")]
    [EmailAddress(ErrorMessage = "El correo no tiene un formato válido.")]
    public string Correo { get; set; }

    [Required]
    public int? UsuarioId { get; set; } = null!;

    [JsonIgnore]
    public virtual ICollection<Reserva> Reservas { get; set; } = new List<Reserva>();

    [JsonIgnore]
    public virtual Usuario Usuario { get; set; }
}
