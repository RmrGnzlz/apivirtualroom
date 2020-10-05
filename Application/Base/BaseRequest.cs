using Application.Mapping;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Base
{
    public abstract class BaseRequest
    {
    }

    public abstract class Request<T> : BaseRequest, IRequest<T> where T : BaseModel
    {
        public abstract T ToEntity();
    }
}
