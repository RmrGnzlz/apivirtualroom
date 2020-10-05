using Application.Mapping;
using Domain.Entities;
using Domain.Values;

namespace Application.Models
{
    public class HorarioModel : Model<Horario>
    {
        public GrupoModel Grupo { get; set; }
        public DiaSemana DiaDeSemana { get; set; }
        public uint HoraInicial { get; set; }
        public uint HoraFinal { get; set; }
        public HorarioModel() { }
        public HorarioModel(Horario entity) : base(entity.Id)
        {
            HoraInicial = entity.HoraInicial;
            HoraFinal = entity.HoraFinal;
            if ((uint)entity.DiaDeSemana < 6)
            {
                DiaDeSemana = (DiaSemana)((uint)entity.DiaDeSemana);
            }
        }
        public override Horario ReverseMap()
        {
            return new Horario
            {
                Id = BaseModel.GetId(Key),
                HoraInicial = HoraInicial,
                HoraFinal = HoraFinal,
                DiaDeSemana = (Domain.Values.DiaDeSemana)((uint)DiaDeSemana),
                Grupo = Grupo != null ? Grupo.ReverseMap() : null
            };
        }
        public HorarioModel Include(Grupo grupo)
        {
            if (grupo != null)
            {
                Grupo = new GrupoModel(grupo);
            }
            return this;
        }
    }
    public enum DiaSemana
    {
        Domingo = 0,
        Lunes = 1,
        Martes = 2,
        Miercoles = 3,
        Jueves = 4,
        Viernes = 5,
        Sabado = 6
    }
}