using Domain.Entities;
using Domain.Repositories;
using Infrastructure.Base;

namespace Infrastructure.Repositories
{
    public class MultimediaRepository : GenericRepository<Multimedia>, IMultimediaRepository
    {
        public MultimediaRepository(IDbContext context) : base(context)
        {
        }
    }
}