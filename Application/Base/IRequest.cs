using Application.Mapping;
using Domain.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Base
{
    public interface IRequest<T> where T : BaseModel
    {
        T ToEntity();
    }
}
