using System.ComponentModel.DataAnnotations;
using Domain.Base;

namespace Domain.Entities
{
    public class Directivo : Entity<int>
    {
        [Required] public Persona Persona { get; set; }
        public Sede Sede { get; set; }
        public string Cargo { get; set; }
    }
}