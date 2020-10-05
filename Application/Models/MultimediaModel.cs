using Application.Mapping;
using Domain.Entities;
using Domain.Values;

namespace Application.Models
{
    public class MultimediaModel : Model<Multimedia>
    {
        public string Uuid { get; set; }
        public string Extension { get; set; }
        public TipoDeMultimedia Tipo { get; set; }
        public MultimediaModel() { }
        public MultimediaModel(Multimedia entity) : base(entity.Id)
        {
            Uuid = entity.Uuid;
            Extension = entity.Extension;
            Tipo = (TipoDeMultimedia)((uint)entity.Tipo);
        }
        public override Multimedia ReverseMap()
        {
            return new Multimedia
            {
                Id = BaseModel.GetId(Key),
                Uuid = Uuid,
                Extension = Extension,
                Tipo = (TipoMultimedia)((uint)Tipo)
            };
        }
    }
    public enum TipoDeMultimedia
    {
        Video = 0, Imagen = 1, Documento = 2
    }
}