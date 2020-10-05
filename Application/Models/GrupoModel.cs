using System;
using System.Collections.Generic;
using Application.Mapping;
using Domain.Entities;
using Domain.Values;

namespace Application.Models
{
    public class GrupoModel : Model<Grupo>
    {
        public string Nombre { get; set; }
        public GradoModel Grado { get; set; }
        public List<GrupoAsignaturaModel> GrupoAsignaturas { get; set; }
        public SedeModel Sede { get; set; }
        public GrupoModel(Grupo entity) : base(entity.Id)
        {
            Nombre = entity.Nombre;
        }
        public override Grupo ReverseMap()
        {
            Grupo grupo = new Grupo
            {
                Id = BaseModel.GetId(Key),
                Nombre = Nombre,
                Grado = Grado != null ? Grado.ReverseMap() : null,
                Sede = Sede != null ? Sede.ReverseMap() : null,
            };
            if (GrupoAsignaturas != null)
            {
                grupo.GrupoAsignaturas = new List<GrupoAsignatura>();
                GrupoAsignaturas.ForEach(x => grupo.GrupoAsignaturas.Add(x.ReverseMap()));
            }
            return grupo;
        }
        public GrupoModel Include(Grado grado)
        {
            if (grado != null)
            {
                Grado = new GradoModel(grado);
            }
            return this;
        }
        public GrupoModel Include(Sede sede)
        {
            if (sede != null)
            {
                Sede = new SedeModel(sede);
            }
            return this;
        }
        public GrupoModel Include(List<GrupoAsignatura> grupoAsignaturas)
        {
            if (grupoAsignaturas != null)
            {
                GrupoAsignaturas = new List<GrupoAsignaturaModel>();
                grupoAsignaturas.ForEach(x => GrupoAsignaturas.Add(new GrupoAsignaturaModel(x)));
            }
            return this;
        }
        public static List<GrupoModel> ListToModels(List<Grupo> grupos)
        {
            if (grupos != null)
            {
                List<GrupoModel> list = new List<GrupoModel>();
                grupos.ForEach(x => list.Add(new GrupoModel(x)));
                return list;
            }
            return null;
        }
    }
}