using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.Base;

namespace Domain.Entities
{
    public class GrupoAsignatura : Entity<int>
    {
        public Asignatura Asignatura { get; set; }
        public Grupo Grupo { get; set; }
        public List<GrupoAsignaturaClase> GrupoAsignaturaClases { get; set; }
        [NotMapped]
        public List<Clase> Clases
        {
            get
            {
                if (GrupoAsignaturaClases == null) return null;
                List<Clase> clases = new List<Clase>(GrupoAsignaturaClases.Capacity);
                GrupoAsignaturaClases.ForEach(x => clases.Add(x.Clase));
                return clases;
            }
        }
        public List<Horario> Horarios { get; set; }
        public Docente Docente { get; set; }

        public void AddClase(Clase clase)
        {
            (GrupoAsignaturaClases ?? (GrupoAsignaturaClases = new List<GrupoAsignaturaClase>())).Add(new GrupoAsignaturaClase
            {
                GrupoAsignatura = this,
                Clase = clase
            });
        }
    }
}