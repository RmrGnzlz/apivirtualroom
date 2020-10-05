using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Base
{
    public interface IResponse<T>
    {
        bool Estado { get; }
        string Mensaje { get;}
        List<T> Data { get; }
    }
}
