using System.Collections.Generic;
using Application.Base;
using Application.Models;
using Domain.Entities;
using Domain.Values;

namespace Application.HttpModel
{
    public class SedeRequest : Request<SedeModel>
    {
        public string NIT { get; set; }
        public string Nombre { get; set; }
        public string Direccion { get; set; }
        public string Telefono { get; set; }
        public List<DirectivoRequest> Directivos { get; set; }
        public override SedeModel ToEntity()
        {
            return new SedeModel
            {
                Nombre = Nombre.Trim().ToUpper(),
                Direccion = (Direccion == null ? string.Empty: Direccion).Trim().ToUpper(),
                Telefono = Telefono
            };
        }
    }
}