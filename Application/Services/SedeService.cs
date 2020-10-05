using Application.Base;
using Application.HttpModel;
using Application.Models;
using Domain.Contracts;
using Domain.Entities;

namespace Application.Services
{
    public class SedeService : Service<Sede>
    {
        public SedeService(IUnitOfWork unitOfWork) : base(unitOfWork, unitOfWork.SedeRepository)
        {
        }

        public BaseResponse Add(SedeRequest request)
        {
            Sede sede = request.ToEntity().ReverseMap();
            sede.Id = 0;
            _repository.Add(sede);
            _unitOfWork.Commit();
            return new Response<SedeModel>(
                mensaje: $"Sede {request.Nombre} registrada exitosamente", 
                entidad: new Models.SedeModel(sede),
                estado: true
            );
        }
    }
}