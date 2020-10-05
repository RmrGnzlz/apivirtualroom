using System.Collections.Generic;
using Application.Mapping;
using Domain.Entities;
using Domain.Values;

namespace Application.Models
{
    public class GradoModel : Model<Grado>
    {
        public string Nombre { get; set; }
        public List<GrupoModel> Grupos { get; set; }
        public GradoModel(Grado entity) : base(entity.Id)
        {
            Nombre = entity.Nombre;
        }
        public override Grado ReverseMap()
        {
            Grado grado = new Grado
            {
                Id = BaseModel.GetId(Key),
                Nombre = Nombre
            };
            if (Grupos != null)
            {
                grado.Grupos = new List<Grupo>();
                Grupos.ForEach(x => grado.Grupos.Add(x.ReverseMap()));
            }
            return grado;
        }
        public GradoModel Include(List<Grupo> grupos)
        {
            if (grupos != null)
            {
                Grupos = new List<GrupoModel>();
                grupos.ForEach(x => Grupos.Add(new GrupoModel(x)));
            }
            return this;
        }
        public static List<GradoModel> ListToModels(List<Grado> grados)
        {
            if (grados != null)
            {
                List<GradoModel> list = new List<GradoModel>();
                grados.ForEach(x => list.Add(new GradoModel(x)));
                return list;
            }
            return null;
        }
    }
}