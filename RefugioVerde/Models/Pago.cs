using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace RefugioVerde.Models;

public partial class Pago
{
    public int IdPago { get; set; }

    [Required(ErrorMessage = "El monto es obligatorio.")]
    [Range(0.01, double.MaxValue, ErrorMessage = "El monto debe ser un valor positivo.")]
    public decimal Monto { get; set; }

    [Required(ErrorMessage = "El método de pago es obligatorio")]
    public int? MetodoDePagoId { get; set; }

    public string Comprobante { get; set; }

    [Required(ErrorMessage = "La reserva es obligatoria.")]
    public int? ReservaId { get; set; }

    public int? EstadoPagoId { get; set; }

    [Required(ErrorMessage = "El tipo de pago es obligatorio.")]
    [StringLength(10, ErrorMessage = "El tipo de pago no puede tener más de 10 caracteres.")]
    public string Tipo { get; set; }

    [Required(ErrorMessage = "La fecha de pago es obligatoria.")]
    public DateTime FechaPago { get; set; }

    [JsonIgnore]
    public virtual EstadoPago EstadoPago { get; set; }

    [JsonIgnore]
    public virtual MetodoDePago MetodoDePago { get; set; }

    [JsonIgnore]
    public virtual Reserva Reserva { get; set; }
}

