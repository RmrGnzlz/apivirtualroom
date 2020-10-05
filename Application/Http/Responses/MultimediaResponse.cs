using System.Collections.Generic;
using Application.Base;
using Domain.Entities;

namespace Application.HttpModel
{
    public class MultimediaResponse : Response<Multimedia>
    {
        public MultimediaResponse(string mensaje, bool estado) : base(mensaje, estado = false)
        {
        }

        public MultimediaResponse(string mensaje, List<Multimedia> entidades, bool estado) : base(mensaje, entidades, estado = false)
        {
        }

        public MultimediaResponse(string mensaje, Multimedia entidad, bool estado) : base(mensaje, entidad, estado = false)
        {
        }
    }
}