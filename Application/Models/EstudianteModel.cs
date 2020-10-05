using System;
using System.Collections.Generic;
using Application.Mapping;
using Domain.Entities;
using Domain.Values;

namespace Application.Models
{
    public class EstudianteModel : Model<Estudiante>
    {
        public PersonaModel DatosPersonales { get; set; }
        public GrupoModel Grupo { get; set; }
        public string Grado { get; set; }
        public EstudianteModel() { }
        public EstudianteModel(Estudiante entity) : base(entity.Id)
        {
            Grado = entity.GradoActual;
        }
        public override Estudiante ReverseMap()
        {
            return new Estudiante
            {
                Id = BaseModel.GetId(Key),
                Persona = DatosPersonales != null ? DatosPersonales.ReverseMap() : null,
                Grupo = Grupo != null ? Grupo.ReverseMap() : null,
                GradoActual = Grado,
            };
        }
        public EstudianteModel Include(Persona persona)
        {
            DatosPersonales = new PersonaModel(persona);
            return this;
        }
        public EstudianteModel Include(Grupo grupo)
        {
            Grupo = new GrupoModel(grupo);
            return this;
        }
        public static List<EstudianteModel> ListToModels(List<Estudiante> estudiantes)
        {
            if (estudiantes != null)
            {
                List<EstudianteModel> list = new List<EstudianteModel>();
                estudiantes.ForEach(x => list.Add(new EstudianteModel(x)));
                return list;
            }
            return null;
        }
    }
}