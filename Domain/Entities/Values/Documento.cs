using System.ComponentModel.DataAnnotations;
using Domain.Base;

namespace Domain.Values {
    public class Documento : Entity<int> {
        [Required] public string TipoDocumento { get; set; }
        [Required, MaxLength(15)] public string NumeroDocumento { get; set; }
    }
}