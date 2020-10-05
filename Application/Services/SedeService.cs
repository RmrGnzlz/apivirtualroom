using Application.Base;
using Domain.Contracts;
using Domain.Entities;

namespace Application.Services
{
    public class SedeService : Service<Sede>
    {
        public SedeService(IUnitOfWork unitOfWork) : base(unitOfWork, unitOfWork.SedeRepository)
        {
        }
    }
}