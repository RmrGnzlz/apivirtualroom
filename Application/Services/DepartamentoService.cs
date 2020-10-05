using Application.Base;
using Domain.Contracts;
using Domain.Entities;

namespace Application.Services
{
    public class DepartamentoService : Service<Departamento>
    {
        public DepartamentoService(IUnitOfWork unitOfWork) : base(unitOfWork, unitOfWork.DepartamentoRepository)
        {
        }
    }
}