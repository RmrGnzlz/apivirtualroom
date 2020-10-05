using System.Collections.Generic;
using Domain.Base;

namespace Domain.Entities
{
    public class Grado : Entity<int>
    {
        public string Nombre { get; set; }
        public List<Grupo> Grupos { get; set; }
    }
}