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

    [Required(ErrorMessage = "El método de pago es obligatorio.")]
    [StringLength(50, ErrorMessage = "El método de pago no puede tener más de 50 caracteres.")]
    public string MetodoPago { get; set; } = null!;

    [Required(ErrorMessage = "El comprobante es obligatorio.")]
    public string Comprobante { get; set; }

    [Required(ErrorMessage = "La reserva es obligatoria.")]
    public int? ReservaId { get; set; }

    public int? EstadoPagoId { get; set; }

    [JsonIgnore]
    public virtual EstadoPago EstadoPago { get; set; }

    [JsonIgnore]
    public virtual Reserva Reserva { get; set; } = null!;
}