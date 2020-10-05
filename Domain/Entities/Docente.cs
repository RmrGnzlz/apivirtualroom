using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Domain.Base;

namespace Domain.Entities
{
    public class Docente : Entity<int>
    {
        [Required] public Persona Persona { get; set; }
        public List<SedeDocente> SedeDocentes { get; set; }
        public List<GradoDocente> GradoDocentes { get; set; }
        public Municipio Municipio { get; set; }
        public List<GrupoAsignatura> GrupoAsignaturas { get; set; }

        public List<Grado> GetGrados()
        {
            if (GradoDocentes == null)
            {
                return null;
            }
            List<Grado> grados = new List<Grado>(GradoDocentes.Capacity);
            GradoDocentes.ForEach(x => grados.Add(x.Grado));
            return grados;
        }
    }
}