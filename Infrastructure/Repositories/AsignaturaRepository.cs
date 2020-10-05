using Domain.Entities;
using Domain.Repositories;
using Infrastructure.Base;

namespace Infrastructure.Repositories
{
    public class AsignaturaRepository : GenericRepository<Asignatura>, IAsignaturaRepository
    {
        public AsignaturaRepository(IDbContext context) : base(context)
        {
        }
    }
}