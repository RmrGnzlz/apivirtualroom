using Application.Base;
using Application.Models;

namespace Application.HttpModel
{
    public class LoginRequest : Request<UsuarioModel>
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public override UsuarioModel ToEntity()
        {
            return new UsuarioModel { Username = Username, Password = Password };
        }
    }
}