using System.Collections.Generic;
using Application.Base;
using Application.Models;
using Domain.Entities;
using Domain.Values;

namespace Application.HttpModel
{
    public class InstitucionRequest : Request<InstitucionModel>
    {
        public string NIT { get; set; }
        public string DANE { get; set; }
        public string Nombre { get; set; }
        public string CodigoMunicipio { get; set; }
        public string PaginaWeb { get; set; }
        public List<SedeRequest> Sedes { get; set; }
        public override InstitucionModel ToEntity()
        {
            var institucion = new InstitucionModel
            {
                Nit = (NIT == null ? string.Empty: NIT).Trim(),
                Dane = (DANE == null ? string.Empty: DANE).Trim(),
                Nombre = (Nombre == null ? string.Empty: Nombre).Trim().ToUpper(),
                PaginaWeb = (PaginaWeb == null ? string.Empty: PaginaWeb).Trim()
            };
            return institucion;
        }
    }
}