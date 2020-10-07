using System.Collections.Generic;
using Application.Base;
using Application.Models;
using Domain.Entities;

namespace Application.HttpModel
{
    public class SedeResponse : Response<SedeModel>
    {
        public SedeResponse(string mensaje, bool estado) : base(mensaje, estado)
        {
        }

        public SedeResponse(string mensaje, List<SedeModel> entidades, bool estado) : base(mensaje, entidades, estado)
        {
        }

        public SedeResponse(string mensaje, SedeModel entidad, bool estado) : base(mensaje, entidad, estado)
        {
        }
    }
}