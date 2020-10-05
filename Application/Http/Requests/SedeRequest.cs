using System.Collections.Generic;
using Application.Base;
using Application.Models;
using Domain.Entities;
using Domain.Values;

namespace Application.HttpModel
{
    public class SedeRequest : Request<SedeModel>
    {
        public int Institucion { get; set; }
        public string Nombre { get; set; }
        public string Direcion { get; set; }
        public string Telefono { get; set; }
        public override SedeModel ToEntity()
        {
            return new SedeModel
            {
                Nombre = Nombre,
                Direccion = Direcion,
                Telefono = Telefono,
                Institucion = new InstitucionModel { Key = Institucion }
            };
        }
    }
}