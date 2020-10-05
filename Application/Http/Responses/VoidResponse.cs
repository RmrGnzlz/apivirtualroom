using Application.Base;

namespace Application.HttpModel
{
    public class VoidResponse : BaseResponse
    {
        public VoidResponse(string mensaje, bool estado)
        {
            Mensaje = mensaje;
            Estado = estado;
        }
    }
}