using System;
using System.Collections.Generic;
using Application.Mapping;
using Domain.Entities;
using Domain.Values;

namespace Application.Models
{
    public class DirectivoModel : Model<Directivo>
    {
        public SedeModel Sede { get; set; }
        public PersonaModel DatosPersonales { get; set; }
        public string Cargo { get; set; }
        public DirectivoModel() { }
        public DirectivoModel(Directivo entity) : base(entity.Id)
        {
            Cargo = entity.Cargo;
        }
        public override Directivo ReverseMap()
        {
            return new Directivo
            {
                Id = BaseModel.GetId(Key),
                Persona = DatosPersonales != null ? DatosPersonales.ReverseMap() : null,
                Cargo = Cargo,
                Sede = Sede == null ? null : Sede.ReverseMap()
            };
        }
        public static List<DirectivoModel> ListToModels(List<Directivo> directivos)
        {
            if (directivos != null)
            {
                List<DirectivoModel> list = new List<DirectivoModel>();
                directivos.ForEach(x => list.Add(new DirectivoModel(x)));
                return list;
            }
            return null;
        }
        public DirectivoModel Include(Sede sede)
        {
            if (sede != null)
            {
                Sede = new SedeModel(sede);
            }
            return this;
        }
        public DirectivoModel Include(Persona persona)
        {
            if (persona != null)
            {
                DatosPersonales = new PersonaModel(persona);
            }
            return this;
        }
    }
}