using Domain.Entities;
using Domain.Repositories;
using Infrastructure.Base;

namespace Infrastructure.Repositories
{
    public class GrupoRepository : GenericRepository<Grupo>, IGrupoRepository
    {
        public GrupoRepository(IDbContext context) : base(context)
        {
        }
    }
}