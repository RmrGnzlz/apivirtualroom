using System;

namespace WebApi.Models
{
    public class TokenJwt
    {
        public string Token { get; set; }
        public DateTime Expiracion { get; set; }
    }
}