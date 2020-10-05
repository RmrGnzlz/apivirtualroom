using System.Collections.Generic;
using Application.Base;
using Application.Models;
using Domain.Entities;
using Domain.Values;

namespace Application.HttpModel
{
    public class MultimediaRequest : Request<MultimediaModel>
    {
        public string Url { get; set; }
        public Application.Models.TipoDeMultimedia Tipo { get; set; }
        public string Extension { get; set; }
        public override MultimediaModel ToEntity()
        {
            return new MultimediaModel
            {
                Uuid = Url,
                Tipo = Tipo,
                Extension = Extension
            };
        }
    }
}