using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace RefugioVerde.Models;

public partial class Empleado
{
    public int EmpleadoId { get; set; }

    [Required(ErrorMessage = "El nombre es obligatorio.")]
    [StringLength(20, MinimumLength = 2, ErrorMessage = "El nombre debe tener entre 2 y 20 caracteres.")]
    public string Nombre { get; set; } = null!;

    [Required(ErrorMessage = "El apellido es obligatorio.")]
    [StringLength(20, MinimumLength = 2, ErrorMessage = "El apellido debe tener entre 2 y 20 caracteres.")]
    public string Apellido { get; set; } = null!;

    [Required(ErrorMessage = "El documento de identidad es obligatorio.")]
    [StringLength(10, MinimumLength = 9, ErrorMessage = "El documento de identidad debe tener entre 9 y 10 dígitos.")]
    [RegularExpression(@"^\d{9,10}$", ErrorMessage = "El documento de identidad debe contener solo números y tener entre 9 y 10 dígitos.")]
    public string DocumentoIdentidad { get; set; } = null!;

    public int MunicipioId { get; set; }

    [Required(ErrorMessage = "El teléfono es obligatorio.")]
    [Phone(ErrorMessage = "El teléfono no tiene un formato válido.")]
    public string Telefono { get; set; }

    [Required(ErrorMessage = "El correo es obligatorio.")]
    [EmailAddress(ErrorMessage = "El correo no tiene un formato válido.")]
    public string Correo { get; set; }

    public int RolId { get; set; }

    [JsonIgnore]
    public virtual Municipio Municipio { get; set; }

    [JsonIgnore]
    public virtual Role Rol { get; set; }

    [JsonIgnore]
    public virtual ICollection<Usuario> Usuarios { get; set; } = new List<Usuario>();
}
