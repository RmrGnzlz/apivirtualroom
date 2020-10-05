using System.Collections.Generic;
using Application.Base;
using Application.Models;
using Domain.Entities;
using Domain.Values;

namespace Application.HttpModel
{
    public class DocenteRequest : Request<DocenteModel>
    {
        public int InstitucionKey { get; set; }
        public string Apellidos { get; set; }
        public string Nombres { get; set; }
        public string NumeroCedula { get; set; }
        public List<DocenteAsignaturaRequest> Asignaturas { get; set; }
        public override DocenteModel ToEntity()
        {
            return new DocenteModel
            {
                DatosPersonales = new PersonaModel
                {
                    Apellidos = Apellidos,
                    Nombres = Nombres,
                    Documento = new DocumentoModel
                    {
                        Tipo = "CEDULA",
                        Numero = NumeroCedula
                    }
                }
            };
        }
    }
    public class DocenteAsignaturaRequest
    {
        public string Asignatura { get; set; }
        public string Grado { get; set; }
        public string Grupo { get; set; }
    }
}