using Domain.Entities;
using Domain.Repositories;
using Infrastructure.Base;

namespace Infrastructure.Repositories
{
    public class DirectivoRepository : GenericRepository<Directivo>, IDirectivoRepository
    {
        public DirectivoRepository(IDbContext context) : base(context)
        {
        }
    }
}