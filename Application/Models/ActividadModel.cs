using System;
using System.Collections.Generic;
using Application.Mapping;
using Domain.Entities;
using Domain.Values;

namespace Application.Models
{
    public class ActividadModel : Model<Actividad>
    {
        public DateTime FechaInicio { get; set; }
        public DateTime? FechaFin { get; set; }
        public ClaseModel Clase { get; set; }
        public List<MultimediaModel> Multimedias { get; set; }
        public ActividadModel() { }
        public ActividadModel(Actividad entity) : base(entity.Id)
        {
            FechaInicio = entity.FechaSubida;
            FechaFin = entity.FechaEntrega;
        }
        public ActividadModel Include(List<Multimedia> multimedia)
        {
            if (multimedia != null)
            {
                Multimedias = new List<MultimediaModel>();
                multimedia.ForEach(x => Multimedias.Add(new MultimediaModel(x)));
            }
            return this;
        }

        public ActividadModel Include(Clase clase)
        {
            if (clase != null)
            {
                ClaseModel claseModel = new ClaseModel(clase);
            }
            return this;
        }

        public override Actividad ReverseMap()
        {
            Actividad actividad = new Actividad
            {
                Id = BaseModel.GetId(Key),
                FechaSubida = FechaInicio,
                FechaEntrega = FechaFin,
            };
            if (Multimedias != null)
            {
                List<Multimedia> multimedia = new List<Multimedia>();
                Multimedias.ForEach(x => multimedia.Add(x.ReverseMap()));
                actividad.Multimedias = multimedia;
            }
            if (Clase != null)
            {
                actividad.Clase = Clase.ReverseMap();
            }
            return actividad;
        }
    }
}