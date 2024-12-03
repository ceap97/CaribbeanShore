using System.ComponentModel.DataAnnotations;

namespace RefugioVerde.Models
{
    public class RestablecerContrasenaModel
    {
        [Required(ErrorMessage = "El correo es obligatorio.")]
        [EmailAddress(ErrorMessage = "El correo no tiene un formato válido.")]
        public string Correo { get; set; }

        [Required(ErrorMessage = "El token es obligatorio.")]
        public string Token { get; set; }

        [Required(ErrorMessage = "La nueva contraseña es obligatoria.")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "La nueva contraseña debe tener entre 6 y 100 caracteres.")]
        public string NuevaContrasena { get; set; }
    }
}
