using System.ComponentModel.DataAnnotations;

namespace RefugioVerde.Models
{
    public class EncuestaSatisfaccion
    {
        public int EncuestaSatisfaccionId { get; set; }

        [Required(ErrorMessage = "La calificación general del hotel es obligatoria.")]
        [Range(1, 5, ErrorMessage = "La calificación debe estar entre 1 y 5.")]
        public int CalificacionGeneral { get; set; }

        [Required(ErrorMessage = "La calificación de la habitación es obligatoria.")]
        [Range(1, 5, ErrorMessage = "La calificación debe estar entre 1 y 5.")]
        public int CalificacionHabitacion { get; set; }

        [Required(ErrorMessage = "La calificación de la atención es obligatoria.")]
        [Range(1, 5, ErrorMessage = "La calificación debe estar entre 1 y 5.")]
        public int CalificacionAtencion { get; set; }

        [StringLength(500, ErrorMessage = "Los comentarios no pueden tener más de 500 caracteres.")]
        public string Comentarios { get; set; }

        [Required]
        public int HabitacionId { get; set; }

        [Required]
        public int EmpleadoId { get; set; }

        public virtual Habitacion Habitacion { get; set; }
        public virtual Empleado Empleado { get; set; }
    }
}
