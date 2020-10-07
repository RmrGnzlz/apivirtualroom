using System.Linq;
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
            Institucion institucion = _unitOfWork.InstitucionRepository.FindFirstOrDefault(x => x.NIT == request.NIT);
            if (institucion == null)
            {
                return new SedeResponse(
                    mensaje: $"Institución con NIT: {request.NIT} no encontrada",
                    entidad: request.ToEntity(),
                    estado: false
                );
            }

            Sede sede = request.ToEntity().ReverseMap();
            institucion.Sedes.Add(sede);

            _repository.Add(sede);
            _unitOfWork.Commit();
            return new Response<SedeModel>(
                mensaje: $"Sede {request.Nombre} registrada exitosamente", 
                entidad: new Models.SedeModel(sede),
                estado: true
            );
        }

        public BaseResponse Search(string busqueda)
        {
            busqueda = busqueda.ToUpper();
            var entities = _repository.FindBy(x => x.Nombre.ToUpper().Contains(busqueda) || x.Telefono.ToUpper().Contains(busqueda) || x.Direccion.ToUpper().Contains(busqueda), false).ToList();
            return new SedeResponse($"Instituciones que coinciden con: {busqueda}", SedeModel.ListToModels(entities), true);
        }
    }
}