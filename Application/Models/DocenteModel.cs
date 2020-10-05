using System;
using System.Collections.Generic;
using Application.Mapping;
using Domain.Entities;

namespace Application.Models
{
    public class DocenteModel : Model<Docente>
    {
        public PersonaModel DatosPersonales { get; set; }
        public MunicipioModel Municipio { get; set; }
        public List<SedeModel> Sedes { get; set; }
        public List<GradoModel> Grados { get; set; }
        public List<GrupoAsignaturaModel> GrupoAsignaturas { get; set; }
        public DocenteModel() { }
        public DocenteModel(Docente entity) : base(entity.Id)
        {
            if (entity.Persona != null)
            {
                DatosPersonales = new PersonaModel(entity.Persona);
            }
        }
        public override Docente ReverseMap()
        {
            Docente docente = new Docente
            {
                Id = BaseModel.GetId(Key),
                Persona = DatosPersonales != null ? DatosPersonales.ReverseMap() : null,
                Municipio = Municipio != null ? Municipio.ReverseMap() : null,
            };
            if (Sedes != null)
            {
                docente.SedeDocentes = new List<SedeDocente>();
                Sedes.ForEach(x => docente.SedeDocentes.Add(new SedeDocente { Docente = docente, Sede = x.ReverseMap() }));
            }
            return docente;
        }
        public DocenteModel Include(Municipio municipio)
        {
            if (municipio != null)
            {
                Municipio = new MunicipioModel(municipio);
            }
            return this;
        }
        public DocenteModel Include(List<Sede> sedes)
        {
            if (sedes != null)
            {
                Sedes = new List<SedeModel>();
                sedes.ForEach(x => Sedes.Add(new SedeModel(x)));
            }
            return this;
        }
        public DocenteModel Include(List<GrupoAsignatura> grupoAsignaturas)
        {
            if (grupoAsignaturas != null)
            {
                GrupoAsignaturas = new List<GrupoAsignaturaModel>();
                grupoAsignaturas.ForEach(x => GrupoAsignaturas.Add(new GrupoAsignaturaModel(x)));
            }
            return this;
        }
        public DocenteModel Include(List<Grado> grados)
        {
            if (grados != null)
            {
                Grados = GradoModel.ListToModels(grados);
            }
            return this;
        }
        public static List<DocenteModel> ListToModels(List<Docente> docentes)
        {
            if (docentes != null)
            {
                List<DocenteModel> list = new List<DocenteModel>();
                docentes.ForEach(x => list.Add(new DocenteModel(x)));
                return list;
            }
            return null;
        }
    }
}