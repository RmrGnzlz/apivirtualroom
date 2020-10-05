using System.Collections.Generic;

namespace Application.Mapping
{
    public abstract class BaseModel
    {
        public static int GetId(int key)
        {
            return key;
        }
        public static int GetKey(int id)
        {
            return id;
        }
    }
}