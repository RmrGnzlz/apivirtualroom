using System;
using System.Collections.Generic;
using Application.Base;
using Application.Models;
using Domain.Entities;
using Domain.Values;

namespace Application.HttpModel
{
    public class ClaseRequest : Request<ClaseModel>
    {
        public string InstitucionNIT { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public List<MultimediaRequest> Multimedias { get; set; }
        public int AsignaturaKey { get; set; }
        public string Grado { get; set; }
        public int PersonaKey { get; set; }
        public override ClaseModel ToEntity()
        {
            return new ClaseModel
            {
                FechaInicio = FechaInicio,
                FechaFin = FechaFin,
                Nombre = Nombre,
                Descripcion = Descripcion,
            };
        }
    }
}