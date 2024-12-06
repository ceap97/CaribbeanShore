using System.ComponentModel.DataAnnotations;

namespace RefugioVerde.Models
{
    public class PagosViewModel : IValidatableObject
    {
        public int IdPago { get; set; }
        public decimal Monto { get; set; }
        public int? MetodoDePagoId { get; set; }
        public string Comprobante { get; set; }
        public int? ReservaId { get; set; }
        public int? EstadoPagoId { get; set; }
        public string Tipo { get; set; }
        public DateTime FechaPago { get; set; }
        public string EstadoPagoNombre { get; set; }
        public string MetodoDePagoNombre { get; set; }
        public string ReservaConfirmacion { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            throw new NotImplementedException();
        }
    }
}
