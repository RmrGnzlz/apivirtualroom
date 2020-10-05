using System.Collections.Generic;
using Application.Mapping;
using Domain.Entities;

namespace Application.Models
{
    public class DepartamentoModel : Model<Departamento>
    {
        public string Codigo { get; set; }
        public string Nombre { get; set; }
        public List<MunicipioModel> Municipios { get; set; }
        public DepartamentoModel(){}
        public DepartamentoModel(Departamento entity) : base(entity.Id)
        {
            Codigo = entity.Codigo;
            Nombre = entity.Nombre;
        }
        public override Departamento ReverseMap()
        {
            Departamento departamento =  new Departamento
            {
                Id = BaseModel.GetId(Key),
                Codigo = Codigo,
                Nombre = Nombre,
            };
            if (Municipios != null)
            {
                var municipios = new List<Municipio>();
                Municipios.ForEach(x => municipios.Add(x.ReverseMap()));
                departamento.Municipios = municipios;
            }
            return departamento;
        }
        public DepartamentoModel Include(List<Municipio> municipios)
        {
            if (municipios != null)
            {
                Municipios = new List<MunicipioModel>();
                municipios.ForEach(x => Municipios.Add(new MunicipioModel(x)));
            }
            return this;
        }
    }
}