using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace RefugioVerde.Models;

public partial class Role
{
    public int RolId { get; set; }

    [Required(ErrorMessage = "El nombre es obligatorio.")]
    [StringLength(30, MinimumLength = 4, ErrorMessage = "El nombre debe tener entre 4 y 30 caracteres.")]
    public string Nombre { get; set; } = null!;

    [JsonIgnore]
    public virtual ICollection<Empleado> Empleados { get; set; } = new List<Empleado>();

    [JsonIgnore]
    public virtual ICollection<Permiso> Permisos { get; set; } = new List<Permiso>();

    [NotMapped]
    public List<int> PermisosSeleccionados { get; set; } = new List<int>();
}

