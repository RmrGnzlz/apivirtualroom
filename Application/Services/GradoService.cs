using Application.Base;
using Domain.Contracts;
using Domain.Entities;

namespace Application.Services
{
    public class GradoService : Service<Grado>
    {
        public GradoService(IUnitOfWork unitOfWork) : base(unitOfWork, unitOfWork.GradoRepository)
        {
        }
    }
}