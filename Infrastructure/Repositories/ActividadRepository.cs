using Domain.Entities;
using Domain.Repositories;
using Infrastructure.Base;

namespace Infrastructure.Repositories
{
    public class ActividadRepository : GenericRepository<Actividad>, IActividadRepository
    {
        public ActividadRepository(IDbContext context) : base(context)
        {
        }
    }
}