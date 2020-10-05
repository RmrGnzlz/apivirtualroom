using System.Collections.Generic;
using Application.Base;
using Application.Models;

namespace Application.HttpModel
{
    public class DirectivoResponse : Response<DirectivoModel>
    {
        public DirectivoResponse(string mensaje, bool estado = false) : base(mensaje, estado)
        {
        }

        public DirectivoResponse(string mensaje, List<DirectivoModel> entidades, bool estado = false) : base(mensaje, entidades, estado)
        {
        }

        public DirectivoResponse(string mensaje, DirectivoModel entidad, bool estado = false) : base(mensaje, entidad, estado)
        {
        }
    }
}