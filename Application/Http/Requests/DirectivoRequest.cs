using System.Collections.Generic;
using Application.Base;
using Application.Models;
using Domain.Entities;
using Domain.Values;

namespace Application.HttpModel
{
    public class DirectivoRequest : IRequest<DirectivoModel>
    {
        public string Sede { get; set; }
        public string Apellidos { get; set; }
        public string Nombres { get; set; }
        public string NumeroCedula { get; set; }
        public string Cargo { get; set; }
        public string Email { get; set; }
        public DirectivoModel ToEntity()
        {
            return new DirectivoModel
            {
                Cargo = Cargo,
                DatosPersonales = new PersonaModel
                {
                    Apellidos = Apellidos,
                    Nombres = Nombres,
                    Documento = new DocumentoModel
                    {
                        Numero = NumeroCedula,
                        Tipo = "CEDULA"
                    }
                }
            };
        }
    }
    public class RegistrarDirectivosRequest : BaseRequest
    {
        public string NIT { get; set; }
        public List<DirectivoRequest> Directivos { get; set; }
    }
}