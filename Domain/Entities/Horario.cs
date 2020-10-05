using System.Collections.Generic;
using Domain.Base;
using Domain.Values;

namespace Domain.Entities
{
    public class Horario : Entity<int>
    {
        public List<Clase> Clases { get; set; }
        public Grupo Grupo { get; set; }
        public DiaDeSemana DiaDeSemana { get; set; }
        public uint HoraInicial { get; set; }
        public uint HoraFinal { get; set; }
    }
}

namespace Domain.Values
{
    public enum DiaDeSemana
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