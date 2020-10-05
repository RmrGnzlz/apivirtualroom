using System.Collections.Generic;
using System.Linq;
using Domain.Entities;
using Domain.Repositories;
using Infrastructure.Base;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class EstudianteRepository : GenericRepository<Estudiante>, IEstudianteRepository
    {
        public EstudianteRepository(IDbContext context) : base(context)
        {
        }

        public IEnumerable<Estudiante> Search(string search)
        {
            return _dbset.Where(x => x.Persona.Nombres.ToUpper().Contains(search.ToUpper()) || x.Persona.Apellidos.ToUpper().Contains(search.ToUpper()) || x.Persona.Documento.NumeroDocumento == search).AsNoTracking().AsEnumerable<Estudiante>();
        }
    }
}