using System.Collections.Generic;
using System.Linq;
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

        public List<Grado> AddRange(List<string> grados)
        {
            grados.ForEach(grado =>
            {
                if (_repository.FindFirstOrDefault(x => x.Nombre == grado) == null)
                {
                    _repository.Add(new Grado { Nombre = grado.ToUpper() });
                }
            });
            _unitOfWork.Commit();
            return _repository.FindBy(x => grados.Contains(x.Nombre), false).ToList();
        }
    }
}