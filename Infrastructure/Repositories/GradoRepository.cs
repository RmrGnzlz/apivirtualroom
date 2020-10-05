using Domain.Entities;
using Domain.Repositories;
using Infrastructure.Base;

namespace Infrastructure.Repositories
{
    public class GradoRepository : GenericRepository<Grado>, IGradoRepository
    {
        public GradoRepository(IDbContext context) : base(context)
        {
        }
    }
}