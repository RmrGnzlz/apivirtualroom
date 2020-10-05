using Domain.Base;

namespace Domain.Entities
{
    public class Multimedia : Entity<int>
    {
        public string Uuid { get; set; }
        public string Extension { get; set; }
        public TipoMultimedia Tipo { get; set; }
    }
    public enum TipoMultimedia
    {
        Video = 0, Image = 1, Document = 2
    }
}