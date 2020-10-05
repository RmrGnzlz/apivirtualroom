using Domain.Base;

namespace Domain.Entities
{
    public class GradoDocente : BaseEntity
    {
        public int DocenteId { get; set; }
        public Docente Docente { get; set; }
        public int GradoId { get; set; }
        public Grado Grado { get; set; }
    }
}