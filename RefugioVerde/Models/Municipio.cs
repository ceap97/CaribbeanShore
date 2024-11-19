using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace RefugioVerde.Models;

public partial class Municipio
{
    public int MunicipioId { get; set; }

    public string Nombre { get; set; } = null!;

    public int DepartamentoId { get; set; }
    [JsonIgnore]

    public virtual ICollection<Cliente> Clientes { get; set; } = new List<Cliente>();
    [JsonIgnore]

    public virtual Departamento Departamento { get; set; } = null!;
    [JsonIgnore]

    public virtual ICollection<Empleado> Empleados { get; set; } = new List<Empleado>();
    [JsonIgnore]

    public virtual ICollection<Huesped> Huespeds { get; set; } = new List<Huesped>();
}
