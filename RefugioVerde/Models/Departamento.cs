using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace RefugioVerde.Models;

public partial class Departamento
{
    public int DepartamentoId { get; set; }

    public string Nombre { get; set; } = null!;
    [JsonIgnore]

    public virtual ICollection<Municipio> Municipios { get; set; } = new List<Municipio>();
}
