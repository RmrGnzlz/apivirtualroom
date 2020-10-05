using System.Collections.Generic;
using Application.Base;
using Application.Models;

namespace Application.HttpModel
{
    public class UsuarioResponse : Response<UsuarioModel>
    {
        public UsuarioResponse(string mensaje, bool estado = false) : base(mensaje, estado)
        {
        }

        public UsuarioResponse(string mensaje, List<UsuarioModel> entidades, bool estado = false) : base(mensaje, entidades, estado)
        {
        }

        public UsuarioResponse(string mensaje, UsuarioModel entidad, bool estado = false) : base(mensaje, entidad, estado)
        {
        }
    }
}