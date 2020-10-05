using System.Collections.Generic;
using Application.Base;
using Application.Mapping;
using Application.Models;

namespace Application.HttpModel
{
    public class LoginModel : BaseModel
    {
        public bool FirstPassword { get; set; }
        public string Rol { get; set; }
        public string Nombre { get; set; }
        public string NombreInstitucion { get; set; }
        public string NITInstitucion { get; set; }
        public string NombreGrado { get; set; }
        public string NombreGrupo { get; set; }
    }
}