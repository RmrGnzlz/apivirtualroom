using System.Collections.Generic;
using Domain.Base;

namespace Application.Mapping
{
    public interface IModel<T>
    {
        T ReverseMap();
    }

    public abstract class Model<T> : BaseModel, IModel<T> where T : class
    {
        protected readonly string _name;
        public int Key { get; set; }
        public Model() : this(0)
        {
        }
        public Model(int id)
        {
            Key = BaseModel.GetKey(id);
        }
        public abstract T ReverseMap();
    }
}