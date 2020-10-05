using System;
using System.Collections.Generic;
using System.Linq;
using Application.Base;
using Application.HttpModel;
using Application.Mapping;
using Application.Models;
using Domain.Contracts;
using Domain.Entities;

namespace Application.Services
{
    public class DocenteService : Service<Docente>
    {
        public DocenteService(IUnitOfWork unitOfWork) : base(unitOfWork, unitOfWork.DocenteRepository)
        {
        }
        public Response<GradoModel> ListaGrados(string numeroDocumento)
        {
            Docente docente = _repository.FindBy(x => x.Persona.Documento.NumeroDocumento == numeroDocumento, includeProperties: "GradoDocentes", trackable: false).FirstOrDefault();
            return new Response<GradoModel>($"Grados del docente {numeroDocumento}", GradoModel.ListToModels(docente.GetGrados()), true);
        }
        public LoginResponse DatosLogin(string username)
        {
            Docente docente = _repository.FindBy(x => x.Persona.Usuario.Username == username, includeProperties: "Persona,Persona.Institucion,Persona.Usuario", trackable: false).FirstOrDefault();
            if (docente == null)
            {
                return new LoginResponse($"No existe el docente con usuario: {username}", null, false);
            }
            if (docente.Persona == null)
            {
                return new LoginResponse($"Error al loguear docente: {username}", null, false);
            }
            return new LoginResponse($"Docente logueado: {username}", new LoginModel
            {
                FirstPassword = string.IsNullOrEmpty(docente.Persona.Usuario.RememberPassword),
                Nombre = $"{docente.Persona.Nombres} {docente.Persona.Apellidos}",
                Rol = TipoUsuarioModel.Docente.ToString(),
                NombreInstitucion = docente.Persona.Institucion.Nombre,
                NITInstitucion = docente.Persona.Institucion.NIT
            }, true);
        }
        public BaseResponse GetAll(int institucionKey)
        {
            return new Response<DocenteModel>("Lista de docentes", DocenteModel.ListToModels(_repository.FindBy(x => x.Persona.Institucion.Id == BaseModel.GetId(institucionKey), includeProperties: "Persona", trackable: false).ToList()), true);
        }
        public BaseResponse Post(DocenteRequest request)
        {
            int institucionId = BaseModel.GetId(request.InstitucionKey);
            Docente docente = request.ToEntity().ReverseMap();
            Institucion institucion = _unitOfWork.InstitucionRepository.FindBy(x => x.Id == institucionId, true).FirstOrDefault();
            if (institucion == null) return new VoidResponse($"La institución {request.InstitucionKey} no se encontró", false);

            request.Asignaturas.ForEach(item =>
            {
                // _unitOfWork.AsignaturaRepository.FindBy(x => x.Nombre == item.Asignatura)
            });
            return null;
        }
        public DocenteModel GetById(int key)
        {
            Docente docente = _repository.FindBy(x => x.Id == BaseModel.GetId(key), includeProperties: "Persona,Persona.Documento").FirstOrDefault();
            return new DocenteModel(docente);
        }
    }
}