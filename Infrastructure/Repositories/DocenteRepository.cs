using Domain.Entities;
using Domain.Repositories;
using Infrastructure.Base;

namespace Infrastructure.Repositories
{
    public class DocenteRepository : GenericRepository<Docente>, IDocenteRepository
    {
        public DocenteRepository(IDbContext context) : base(context)
        {
        }
    }
}