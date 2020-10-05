using Application.Base;

namespace Application.HttpModel
{
    public class LoginResponse : Response<LoginModel>
    {
        public object Auth { get; set; }
        public LoginResponse(string mensaje, LoginModel entidad, bool estado) : base(mensaje, entidad, estado)
        {
        }
    }
}