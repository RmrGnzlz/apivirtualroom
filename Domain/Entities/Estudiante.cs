using System.ComponentModel.DataAnnotations;
using Domain.Base;

namespace Domain.Entities
{
    public class Estudiante : Entity<int>
    {
        [Required] public Persona Persona { get; set; }
        [Required] public Sede Sede { get; set; }
        [Required] public Grupo Grupo { get; set; }
        public string GradoActual { get; set; }
    }
}