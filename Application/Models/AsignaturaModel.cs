using System;
using System.Collections.Generic;
using Application.Mapping;
using Domain.Entities;

namespace Application.Models
{
    public class AsignaturaModel : Model<Asignatura>
    {
        public string Nombre { get; set; }
        public InstitucionModel Institucion { get; set; }
        public List<GradoModel> Grados { get; set; }
        public AsignaturaModel() { }
        public AsignaturaModel(Asignatura entity) : base(entity.Id)
        {
            Nombre = entity.Nombre;
        }
        public override Asignatura ReverseMap()
        {
            return new Asignatura
            {
                Id = BaseModel.GetId(Key),
                Nombre = Nombre,
                Institucion = Institucion.ReverseMap(),
            };
        }
        public AsignaturaModel Include(Institucion institucion)
        {
            if (institucion != null)
            {
                Institucion = new InstitucionModel(institucion);
            }
            return this;
        }
        public AsignaturaModel Include(List<Grado> grados)
        {
            if (grados != null)
            {
                Grados = new List<GradoModel>();
                grados.ForEach(x => Grados.Add(new GradoModel(x)));
            }
            return this;
        }

        internal static List<AsignaturaModel> ListToModels(List<Asignatura> asignaturas)
        {
            if (asignaturas == null) return null;
            List<AsignaturaModel> asignaturaModels = new List<AsignaturaModel>(asignaturas.Capacity);

            asignaturas.ForEach(x => asignaturaModels.Add(new AsignaturaModel(x)));

            return asignaturaModels;
        }
    }
}