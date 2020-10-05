using Domain.Base;

namespace Domain.Entities
{
    public class GrupoAsignaturaClase : BaseEntity
    {
        public int GrupoAsignaturaId { get; set; }
        public GrupoAsignatura GrupoAsignatura { get; set; }
        public int ClaseId { get; set; }
        public Clase Clase { get; set; }
    }
}