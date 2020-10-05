using Domain.Entities;
using Domain.Repositories;
using Infrastructure.Base;

namespace Infrastructure.Repositories
{
    public class SedeRepository : GenericRepository<Sede>, ISedeRepository
    {
        public SedeRepository(IDbContext context) : base(context)
        {
        }
    }
}