using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace RefugioVerde.Models;

public partial class Permiso
{
    public int PermisoId { get; set; }

    [Required(ErrorMessage = "El nombre es obligatorio.")]
    [StringLength(50, ErrorMessage = "El nombre no puede tener más de 50 caracteres.")]
    public string Nombre { get; set; } = null!;

    [StringLength(200, ErrorMessage = "La descripción no puede tener más de 200 caracteres.")]
    public string Descripcion { get; set; }

    [JsonIgnore]
    public virtual ICollection<Role> Rols { get; set; } = new List<Role>();
}
