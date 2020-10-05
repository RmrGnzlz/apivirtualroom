using Domain.Entities;
using Domain.Repositories;
using Infrastructure.Base;

namespace Infrastructure.Repositories
{
    public class GrupoAsignaturaClaseRepository : GenericRepository<GrupoAsignaturaClase>, IGrupoAsignaturaClaseRepository
    {
        public GrupoAsignaturaClaseRepository(IDbContext context) : base(context)
        {
        }
    }
}