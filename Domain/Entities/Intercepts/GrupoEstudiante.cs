using Domain.Base;

namespace Domain.Entities
{
    public class GrupoEstudiante : BaseEntity
    {
        public int GrupoId { get; set; }
        public Grupo Grupo { get; set; }
        public int EstudianteId { get; set; }
        public Estudiante Estudiante { get; set; }
    }
}