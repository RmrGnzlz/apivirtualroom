using System.Collections.Generic;
using Application.Mapping;
using Domain.Entities;
using Domain.Values;

namespace Application.Models
{
    public class InstitucionModel : Model<Institucion>
    {
        public MunicipioModel Municipio { get; set; }
        public List<SedeModel> Sedes { get; set; }
        public List<AsignaturaModel> Asignaturas { get; set; }
        public string Nit { get; set; }
        public string Dane { get; set; }
        public string Nombre { get; set; }
        public string PaginaWeb { get; set; }
        public InstitucionModel() { }
        public InstitucionModel(Institucion entity) : base(entity.Id)
        {
            Nit = entity.NIT;
            Dane = entity.DANE;
            Nombre = entity.Nombre;
            PaginaWeb = entity.PaginaWeb;
        }
        public override Institucion ReverseMap()
        {
            return new Institucion
            {
                Id = GetId(Key),
                NIT = Nit,
                DANE = Dane,
                Nombre = Nombre,
                PaginaWeb = PaginaWeb,
            };
        }
        public InstitucionModel Include(List<Sede> sedes)
        {
            Sedes = SedeModel.ListToModels(sedes);
            return this;
        }
        public InstitucionModel Include(List<Asignatura> asignaturas)
        {
            Asignaturas = AsignaturaModel.ListToModels(asignaturas);
            return this;
        }
        public InstitucionModel Include(Municipio municipio)
        {
            if (municipio != null)
            {
                Municipio = new MunicipioModel(municipio);
            }
            return this;
        }
        public static List<InstitucionModel> ListToModels(List<Institucion> entities)
        {
            if (entities != null)
            {
                List<InstitucionModel> list = new List<InstitucionModel>();
                entities.ForEach(x => list.Add(new InstitucionModel(x)));
                return list;
            }
            return null;
        }
    }
}