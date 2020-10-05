using System.Collections.Generic;
using Application.Base;
using Application.Models;
using Domain.Entities;

namespace Application.HttpModel
{
    public class InstitucionResponse : Response<InstitucionModel>
    {
        public InstitucionResponse(string mensaje, bool estado) : base(mensaje, estado)
        {
        }

        public InstitucionResponse(string mensaje, List<InstitucionModel> entidades, bool estado) : base(mensaje, entidades, estado)
        {
        }

        public InstitucionResponse(string mensaje, InstitucionModel entidad, bool estado) : base(mensaje, entidad, estado)
        {
        }
    }
}