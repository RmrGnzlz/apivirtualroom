using System.Collections.Generic;
using Application.Base;
using Application.Models;

namespace Application.HttpModel
{
    public class GrupoRequest : BaseRequest
    {
        public string CedulaDocente { get; set; }
        public List<string> Asignaturas { get; set; }
        public List<string> Grados { get; set; }
        public List<string> Grupos { get; set; }
    }
    public class RegistroGruposRequest : BaseRequest
    {
        public string NIT { get; set; }
        public List<GrupoRequest> Grupos { get; set; }
    }
}