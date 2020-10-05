using Domain.Base;

namespace Domain.Entities
{
    public class GradoAsignatura : BaseEntity
    {
        public int GradoId { get; set; }
        public Grado Grado { get; set; }
        public int AsignaturaId { get; set; }
        public Asignatura Asignatura { get; set; }
    }
}