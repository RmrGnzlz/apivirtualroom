using System.Collections.Generic;
using Application.Base;
using Application.Models;
using Domain.Entities;
using Domain.Values;

namespace Application.HttpModel
{
    public class ActividadRequest : Request<ActividadModel>
    {
        public string Grado { get; set; }
        public string Grupo { get; set; }
        public List<string> Uuids { get; set; }
        public override ActividadModel ToEntity()
        {
            var multimedia = new List<MultimediaModel>();
            Uuids.ForEach(x => multimedia.Add(
                new MultimediaModel { Uuid = x }
            ));
            return new ActividadModel { Multimedias = multimedia };
        }
    }
}