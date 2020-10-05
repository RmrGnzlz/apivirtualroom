using System.Collections.Generic;
using Domain.Base;

namespace Domain.Entities
{
    public class Municipio : Entity<int>
    {
        public Departamento Departamento { get; set; }
        public string Codigo { get; set; }
        public string Nombre { get; set; }
        public List<Institucion> Instituciones { get; set; }
    }
}