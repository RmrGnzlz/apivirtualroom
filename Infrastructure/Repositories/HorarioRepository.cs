using Domain.Entities;
using Domain.Repositories;
using Infrastructure.Base;

namespace Infrastructure.Repositories
{
    public class HorarioRepository : GenericRepository<Horario>, IHorarioRepository
    {
        public HorarioRepository(IDbContext context) : base(context)
        {
        }
    }
}