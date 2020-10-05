using System;
using System.Collections.Generic;
using Application.Mapping;
using Domain.Entities;

namespace Application.Models
{
    public class ClaseModel : Model<Clase>
    {
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public HorarioModel Horario { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public List<MultimediaModel> Multimedias { get; set; }
        public List<GrupoAsignaturaModel> GrupoAsignaturas { get; set; }
        public List<ActividadModel> Actividades { get; set; }
        public ClaseModel() { }
        public ClaseModel(Clase entity) : base(entity.Id)
        {
            FechaInicio = entity.FechaInicio;
            FechaFin = entity.FechaCierre;
            Nombre = entity.Nombre;
            Descripcion = entity.Descripcion;
        }
        public override Clase ReverseMap()
        {
            Clase clase = new Clase
            {
                Id = BaseModel.GetId(Key),
                Nombre = Nombre,
                Descripcion = Descripcion,
                FechaInicio = FechaInicio,
                FechaCierre = FechaFin,
            };
            if (GrupoAsignaturas != null)
            {
                GrupoAsignaturas.ForEach(x => {
                    clase.AddGrupoAsignatura(x.ReverseMap());
                });
            }
            if (Actividades != null)
            {
                clase.Actividades = new List<Actividad>();
                Actividades.ForEach(x => clase.Actividades.Add(x.ReverseMap()));
            }
            if (Multimedias != null)
            {
                clase.Multimedias = new List<Multimedia>();
                Multimedias.ForEach(x => clase.Multimedias.Add(x.ReverseMap()));
            }
            return clase;
        }
        public ClaseModel Include(List<GrupoAsignatura> grupoAsignaturas)
        {
            if (grupoAsignaturas != null)
            {
                GrupoAsignaturas = new List<GrupoAsignaturaModel>(grupoAsignaturas.Capacity);
                grupoAsignaturas.ForEach(x => GrupoAsignaturas.Add(new GrupoAsignaturaModel(x)));
            }
            return this;
        }
        public void Include(GrupoAsignatura grupoAsignatura)
        {
            (GrupoAsignaturas ?? (GrupoAsignaturas = new List<GrupoAsignaturaModel>())).Add(new GrupoAsignaturaModel(grupoAsignatura));
        }
        public ClaseModel Include(List<Actividad> actividades)
        {
            if (actividades != null)
            {
                Actividades = new List<ActividadModel>();
                actividades.ForEach(x => Actividades.Add(new ActividadModel(x)));
            }
            return this;
        }
        public ClaseModel Include(Horario horario)
        {
            if (horario != null)
            {
                Horario = new HorarioModel(horario);
            }
            return this;
        }
        public ClaseModel Include(List<Multimedia> multimedias)
        {
            if (multimedias != null)
            {
                Multimedias = new List<MultimediaModel>(multimedias.Capacity);
                multimedias.ForEach(x => Multimedias.Add(new MultimediaModel(x)));
            }
            return this;
        }
        internal static List<ClaseModel> ListToModels(List<Clase> clases)
        {
            if (clases == null) return null;
            List<ClaseModel> clasesModel = new List<ClaseModel>(clases.Capacity);
            clases.ForEach(x => clasesModel.Add(new ClaseModel(x)));
            return clasesModel;
        }
    }
}