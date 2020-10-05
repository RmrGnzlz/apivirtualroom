using Domain.Entities;
using Domain.Repositories;
using Infrastructure.Base;

namespace Infrastructure.Repositories
{
    public class ClaseRepository : GenericRepository<Clase>, IClaseRepository
    {
        public ClaseRepository(IDbContext context) : base(context)
        {
        }
    }
}