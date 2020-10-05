using Domain.Entities;
using Domain.Repositories;
using Infrastructure.Base;

namespace Infrastructure.Repositories
{
    public class InstitucionRepository : GenericRepository<Institucion>, IInstitucionRepository
    {
        public InstitucionRepository(IDbContext context) : base(context)
        {
        }
    }
}