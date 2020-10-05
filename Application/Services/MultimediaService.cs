using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Application.Base;
using Application.HttpModel;
using Domain.Contracts;
using Domain.Entities;

namespace Application.Services
{
    public class MultimediaService : Service<Multimedia>
    {
        public MultimediaService(IUnitOfWork unitOfWork) : base(unitOfWork, unitOfWork.MultimediaRepository)
        {
        }
        // public BaseResponse Post(MultimediaRequest request)
        // {
        //     if (ExisteRecurso(request.Url)) return new MultimediaResponse($"El recurso {request.Url} ya existe.", false);

        //     Multimedia multimedia = request.ToEntity().ReverseMap();
        //     // Condición con la que buscaremos los grupos-asignatura
        //     Expression<Func<GrupoAsignatura, bool>> condicion = x => request.Grupos.Contains(x.Grupo.Id);

        //     int cantidad = _unitOfWork.GrupoAsignaturaRepository.Count(condicion);
        //     if (cantidad != request.Grupos.Count())
        //     {
        //         // Hay grupos que no se encuentran registrados en la base de datos, por lo que no registraremos la multimedia.
        //         return new VoidResponse("Algunos grupos no se encuentran registrados en la base de datos.", false);
        //     }

        //     List<GrupoAsignatura> grupos = _unitOfWork.GrupoAsignaturaRepository.FindBy(condicion, false).ToList();
        //     grupos.ForEach(x =>
        //     {
        //         // x.AgregarMultimedia(multimedia);
        //         _unitOfWork.GrupoAsignaturaRepository.Edit(x);
        //     });

        //     int saves = _unitOfWork.Commit();
        //     return new MultimediaResponse($"{saves} datos guardados correctamente", multimedia, true);
        // }
        public MultimediaResponse GetAll(uint page, uint size)
        {
            return new MultimediaResponse("Registros consultados", base.Get(page: page,  size: size), true);
        }
        public bool ExisteRecurso(string url)
        {
            return _repository.FindFirstOrDefault(x => x.Uuid == url) != null;
        }
    }
}