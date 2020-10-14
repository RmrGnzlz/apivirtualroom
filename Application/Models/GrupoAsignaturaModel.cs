using System;
using System.Collections.Generic;
using Application.Mapping;
using Domain.Entities;
using Domain.Values;

namespace Application.Models
{
    public class GrupoAsignaturaModel : Model<GrupoAsignatura>
    {
        public AsignaturaModel Asignatura { get; set; }
        public GrupoModel Grupo { get; set; }
        public DocenteModel Docente { get; set; }
        public List<ClaseModel> Clases { get; set; }
        public List<HorarioModel> Horarios { get; set; }
        public GrupoAsignaturaModel(GrupoAsignatura entity) : base(entity.Id)
        {
        }
        public override GrupoAsignatura ReverseMap()
        {
            GrupoAsignatura grupoAsignatura = new GrupoAsignatura
            {
                Id = BaseModel.GetId(Key),
                Grupo = Grupo != null ? Grupo.ReverseMap() : null,
                Docente = Docente != null ? Docente.ReverseMap() : null,
                Asignatura = Asignatura != null ? Asignatura.ReverseMap() : null,
            };
            if (Clases != null)
            {
                Clases.ForEach(x => grupoAsignatura.AddClase(x.ReverseMap()));
            }
            if (Horarios != null)
            {
                grupoAsignatura.Horarios = new List<Horario>();
                Horarios.ForEach(x => grupoAsignatura.Horarios.Add(x.ReverseMap()));
            }
            return grupoAsignatura;
        }
        public GrupoAsignaturaModel Include(Asignatura asignatura)
        {
            if (asignatura != null)
            {
                Asignatura = new AsignaturaModel(asignatura);
            }
            return this;
        }
        public GrupoAsignaturaModel Include(Grupo grupo)
        {
            if (grupo != null)
            {
                Grupo = new GrupoModel(grupo);
            }
            return this;
        }
        public GrupoAsignaturaModel Include(Docente docente)
        {
            if (docente != null)
            {
                Docente = new DocenteModel(docente);
            }
            return this;
        }
        public GrupoAsignaturaModel Include(List<Clase> clases)
        {
            if (clases != null)
            {
                Clases = new List<ClaseModel>();
                clases.ForEach(x => Clases.Add(new ClaseModel(x)));
            }
            return this;
        }
        public GrupoAsignaturaModel Include(List<Horario> horarios)
        {
            if (horarios != null)
            {
                Horarios = new List<HorarioModel>();
                horarios.ForEach(x => Horarios.Add(new HorarioModel(x)));
            }
            return this;
        }

        public static List<GrupoAsignaturaModel> ListToModels(List<GrupoAsignatura> grupoAsignaturas)
        {
            List<GrupoAsignaturaModel> grupoAsignaturaModels = new List<GrupoAsignaturaModel>(grupoAsignaturas.Count);
            grupoAsignaturas.ForEach(x => {
                grupoAsignaturaModels.Add(new GrupoAsignaturaModel(x).Include(x.Asignatura).Include(x.Grupo));
            });
            return grupoAsignaturaModels;
        }
    }
}