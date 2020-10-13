using System.Collections.Generic;
using Domain.Base;

namespace Domain.Entities
{
    public class Institucion : Entity<int>
    {
        public Municipio Municipio { get; set; }
        public List<Sede> Sedes { get; set; }
        public List<Asignatura> Asignaturas { get; set; }
        public string NIT { get; set; }
        public string DANE { get; set; }
        public string Nombre { get; set; }
        public string PaginaWeb { get; set; }
    }
}