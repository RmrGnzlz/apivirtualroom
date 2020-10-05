using System.Collections.Generic;
using Domain.Base;

namespace Domain.Entities
{
    public class Sede : Entity<int>
    {
        public Institucion Institucion { get; set; }
        public string Nombre { get; set; }
        public string Direccion { get; set; }
        public string Telefono { get; set; }
        public List<Directivo> Directivos { get; set; }
        public List<Estudiante> Estudiantes { get; set; }
        public List<SedeDocente> SedeDocentes { get; set; }
        public List<Grupo> Grupos { get; set; }
    }
}