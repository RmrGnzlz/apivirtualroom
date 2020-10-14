using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Domain.Base;

namespace Domain.Entities
{
    public class Asignatura : Entity<int>
    {
        public Institucion Institucion { get; set; }
        public string Nombre { get; set; }
        public List<GradoAsignatura> GradoAsignaturas { get; set; }
        public List<GrupoAsignatura> GrupoAsignaturas { get; set; }

        public void AgregarGrados(List<Grado> grados)
        {
            if (GradoAsignaturas == null)
            {
                GradoAsignaturas = new List<GradoAsignatura>();
            }
            grados.ForEach(x => {
                GradoAsignaturas.Add(new GradoAsignatura
                {
                    Asignatura = this,
                    Grado = x
                });
            });
        }
    }
}