using System.Collections.Generic;
using Application.Mapping;
using Domain.Entities;
using Domain.Values;

namespace Application.Models
{
    public class SedeModel : Model<Sede>
    {
        public InstitucionModel Institucion { get; set; }
        public string Nombre { get; set; }
        public string Direccion { get; set; }
        public string Telefono { get; set; }
        public List<DirectivoModel> Directivos { get; set; }
        public List<EstudianteModel> Estudiantes { get; set; }
        public List<DocenteModel> Docentes { get; set; }
        public List<GrupoModel> Grupos { get; set; }
        public SedeModel() { }
        public SedeModel(Sede entity) : base(entity.Id)
        {
            Nombre = entity.Nombre;
            Direccion = entity.Direccion;
            Telefono = entity.Telefono;
        }
        public override Sede ReverseMap()
        {
            return new Sede();
        }
        public SedeModel Include(List<Directivo> directivos)
        {
            Directivos = DirectivoModel.ListToModels(directivos);
            return this;
        }
        public SedeModel Include(List<Estudiante> estudiantes)
        {
            Estudiantes = EstudianteModel.ListToModels(estudiantes);
            return this;
        }
        public SedeModel Include(List<Docente> docentes)
        {
            Docentes = DocenteModel.ListToModels(docentes);
            return this;
        }
        public SedeModel Include(List<Grupo> grupos)
        {
            Grupos = GrupoModel.ListToModels(grupos);
            return this;
        }
        public static List<SedeModel> ListToModels(List<Sede> sedes)
        {
            if (sedes != null)
            {
                List<SedeModel> list = new List<SedeModel>();
                sedes.ForEach(x => list.Add(new SedeModel(x)));
                return list;
            }
            return null;
        }
    }
}