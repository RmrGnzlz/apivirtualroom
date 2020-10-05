using System.Collections.Generic;
using System.Linq;
using Application.Base;
using Application.Models;

namespace Application.HttpModel
{
    public class AsignaturaDocenteResponse : Response<AsignaturaDocente>
    {
        public AsignaturaDocenteResponse(string mensaje, bool estado, List<GrupoAsignaturaModel> grupoAsignaturas) : base(mensaje, estado)
        {
            List<AsignaturaDocente> asignaturas = new List<AsignaturaDocente>();
            grupoAsignaturas.ForEach(x => {
                AsignaturaDocente asignatura = asignaturas.Where(y => y.Asignatura.Nombre == x.Asignatura.Nombre && y.Asignatura.Key == x.Asignatura.Key).FirstOrDefault();
                if (asignatura == null)
                {
                    asignaturas.Add(new AsignaturaDocente {
                        Asignatura = x.Asignatura,
                        Grados = new List<string>{ x.Grupo.Grado.Nombre }
                    });
                }
                else
                {
                    if (!asignatura.Grados.Contains(x.Grupo.Grado.Nombre))
                    {
                        asignatura.Grados.Add(x.Grupo.Grado.Nombre);
                    }
                }
            });
            Data = asignaturas;
        }
    }
    public class AsignaturaDocente
    {
        public AsignaturaModel Asignatura { get; set; }
        public List<string> Grados { get; set; }
    }
}