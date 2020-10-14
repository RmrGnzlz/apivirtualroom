using System.Collections.Generic;
using Domain.Base;

namespace Domain.Entities
{
    public class Grado : Entity<int>
    {
        public string Nombre { get; set; }
        public List<Grupo> Grupos { get; set; }
        public List<GradoAsignatura> GradoAsignaturas { get; set; }

        public List<Asignatura> Asignaturas()
        {
            if (GradoAsignaturas == null) return null;
            List<Asignatura> asignaturas = new List<Asignatura>(GradoAsignaturas.Count);
            GradoAsignaturas.ForEach(x => {
                asignaturas.Add(x.Asignatura);
            });
            return asignaturas;
        }
    }
}