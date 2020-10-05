using Application.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Base
{
    public abstract class BaseResponse
    {
        public bool Estado { get; set; }
        public string Mensaje { get; set; }
    }
    public class Response<T> : BaseResponse, IResponse<T> where T : class
    {
        public List<T> Data { get; protected set; }

        public Response(string mensaje, List<T> data, bool estado)
        {
            Mensaje = mensaje;
            Data = data;
            Estado = estado;
        }

        public Response(string mensaje, T entidad, bool estado)
        {
            Mensaje = mensaje;
            Data = new List<T> { entidad };
            Estado = estado;
        }

        public Response(string mensaje, bool estado)
        {
            Mensaje = mensaje;
            Estado = estado;
        }
    }
}
