using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.Base;

namespace Domain.Entities
{
    public class Clase : Entity<int>
    {
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public Horario Horario { get; set; }
        public Asignatura Asignatura { get; set; }
        public List<GrupoAsignaturaClase> GrupoAsignaturaClases { get; set; }
        [NotMapped]
        public List<GrupoAsignatura> GrupoAsignaturas
        {
            get
            {
                if (GrupoAsignaturaClases == null) return null;
                List<GrupoAsignatura> grupoAsignaturas = new List<GrupoAsignatura>(GrupoAsignaturaClases.Capacity);
                GrupoAsignaturaClases.ForEach(x => grupoAsignaturas.Add(x.GrupoAsignatura));
                return grupoAsignaturas;
            }
        }
        public DateTime FechaInicio { get; set; }
        public List<Actividad> Actividades { get; set; }
        public List<Multimedia> Multimedias { get; set; }
        public DateTime FechaCierre { get; set; }

        public void AddMultimedia(Multimedia multimedia)
        {
            if (multimedia != null)
            {
                if (Multimedias == null) Multimedias = new List<Multimedia>();
                Multimedias.Add(multimedia);
            }
        }
        public void AddGrupoAsignatura(GrupoAsignatura grupoAsignatura)
        {
            if (grupoAsignatura != null)
            {
                (GrupoAsignaturaClases ?? (GrupoAsignaturaClases = new List<GrupoAsignaturaClase>())).Add(new GrupoAsignaturaClase { Clase = this, GrupoAsignatura = grupoAsignatura });
            }
        }
    }
}