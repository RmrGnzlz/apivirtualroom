using System.ComponentModel.DataAnnotations;
using Domain.Base;
using Domain.Values;

namespace Domain.Entities
{
    public class Persona : Entity<int>
    {
        [Required] public string Nombres { get; set; }
        [Required] public string Apellidos { get; set; }
        [Required] public Documento Documento { get; set; }
        public virtual Usuario Usuario { get; set; }
        public Institucion Institucion { get; set; }
    }
}