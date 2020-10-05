using Domain.Base;

namespace Domain.Entities
{
    public class SedeDocente : BaseEntity
    {
        public int DocenteId { get; set; }
        public Docente Docente { get; set; }
        public int SedeId { get; set; }
        public Sede Sede { get; set; }
    }
}