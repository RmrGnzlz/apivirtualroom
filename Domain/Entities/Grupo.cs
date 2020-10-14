using Domain.Base;
using System;
using System.Collections.Generic;

namespace Domain.Entities
{
    public class Grupo : Entity<int>
    {
        public string Nombre { get; set; }
        public Grado Grado { get; set; }
        public List<GrupoAsignatura> GrupoAsignaturas { get; set; }
        public List<Estudiante> Estudiantes { get; set; }
        public Sede Sede { get; set; }

        public void AddEstudiante(Estudiante estudiante)
        {
            if (estudiante != null)
            {
                if (Estudiantes == null) Estudiantes = new List<Estudiante>();
                Estudiantes.Add(estudiante);
            }
        }
        public List<GrupoAsignatura> AddAsignaturas(List<Asignatura> asignaturas, Docente docente)
        {
            if (GrupoAsignaturas == null)
            {
                GrupoAsignaturas = new List<GrupoAsignatura>();
            }
            asignaturas.ForEach(x =>
            {
                GrupoAsignaturas.Add(new GrupoAsignatura { Asignatura = x, Docente = docente });
            });
            return GrupoAsignaturas;
        }
        public List<Asignatura> Asignaturas()
        {
            if (GrupoAsignaturas == null) return null;
            List<Asignatura> asignaturas = new List<Asignatura>(GrupoAsignaturas.Capacity);
            GrupoAsignaturas.ForEach(x => asignaturas.Add(x.Asignatura));
            return asignaturas;
        }
    }
}