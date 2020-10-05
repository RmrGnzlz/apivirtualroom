using Application.Base;
using Domain.Contracts;
using Domain.Entities;

namespace Application.Services
{
    public class GrupoService : Service<Grupo>
    {
        public GrupoService(IUnitOfWork unitOfWork) : base(unitOfWork, unitOfWork.GrupoRepository)
        {
        }
    }
}