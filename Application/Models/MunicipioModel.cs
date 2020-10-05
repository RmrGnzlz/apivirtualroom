using System.Collections.Generic;
using Application.Mapping;
using Domain.Entities;
using Domain.Values;

namespace Application.Models
{
    public class MunicipioModel : Model<Municipio>
    {
        public DepartamentoModel Departamento { get; set; }
        public string Codigo { get; set; }
        public string Nombre { get; set; }
        public List<InstitucionModel> Instituciones { get; set; }
        public MunicipioModel() { }
        public MunicipioModel(Municipio entity) : base(entity.Id)
        {
            Codigo = entity.Codigo;
            Nombre = entity.Nombre;
        }
        public override Municipio ReverseMap()
        {
            return new Municipio {
                Codigo = Codigo,
                Nombre = Nombre,
            };
        }
        public MunicipioModel Include(Departamento departamento)
        {
            if (departamento != null)
            {
                Departamento = new DepartamentoModel(departamento);
            }
            return this;
        }
        public MunicipioModel Include(List<Institucion> instituciones)
        {
            Instituciones = InstitucionModel.ListToModels(instituciones);
            return this;
        }
    }
}