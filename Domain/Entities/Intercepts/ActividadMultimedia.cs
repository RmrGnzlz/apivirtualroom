using Domain.Base;

namespace Domain.Entities
{
    public class ActividadMultimedia : BaseEntity
    {
        public int ActividadId { get; set; }
        public Actividad Actividad { get; set; }
        public int MultimediaId { get; set; }
        public Multimedia Multimedia { get; set; }
    }
}