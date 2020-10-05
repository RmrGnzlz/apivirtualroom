using Domain.Entities;
using Domain.Repositories;
using Infrastructure.Base;

namespace Infrastructure.Repositories
{
    public class GrupoAsignaturaRepository : GenericRepository<GrupoAsignatura>, IGrupoAsignaturaRepository
    {
        public GrupoAsignaturaRepository(IDbContext context) : base(context)
        {
        }
    }
}