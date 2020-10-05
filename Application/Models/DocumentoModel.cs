using Application.Mapping;
using Domain.Values;

namespace Application.Models
{
    public class DocumentoModel : Model<Documento>
    {
        public string Numero { get; set; }
        public string Tipo { get; set; }
        public DocumentoModel() { }
        public DocumentoModel(Documento entity) : base(entity.Id)
        {
            Numero = entity.NumeroDocumento;
            Tipo = entity.TipoDocumento;
        }
        public override Documento ReverseMap()
        {
            return new Documento
            {
                Id = BaseModel.GetId(Key),
                NumeroDocumento = Numero,
                TipoDocumento = Tipo,
            };
        }
    }
}