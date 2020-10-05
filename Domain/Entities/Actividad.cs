using System;
using System.Collections.Generic;
using Domain.Base;

namespace Domain.Entities
{
    public class Actividad : Entity<int>
    {
        public Clase Clase { get; set; }
        public DateTime FechaSubida { get; set; }
        public List<Multimedia> Multimedias { get; set; }
        public DateTime? FechaEntrega { get; set; }
    }
}