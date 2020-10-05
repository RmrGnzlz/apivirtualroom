using System.Collections.Generic;
using Domain.Base;

namespace Domain.Entities
{
    public class Departamento : Entity<int>
    {
        public string Codigo { get; set; }
        public string Nombre { get; set; }
        public IEnumerable<Municipio> Municipios { get; set; }
    }
}