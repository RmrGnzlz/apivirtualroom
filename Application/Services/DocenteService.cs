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
            List<Docente> docentes = _repository.FindBy(x => x.Persona.Institucion.Id == BaseModel.GetId(institucionKey), includeProperties: "Persona", trackable: false).ToList();
            return new Response<DocenteModel>($"Lista de docentes ({docentes.Count})", DocenteModel.ListToModels(docentes), true);
        }
        public BaseResponse PostRange(List<DocenteRequest> request, string NIT)
        {
            Institucion institucion = _unitOfWork.InstitucionRepository.FindFirstOrDefault(x => x.NIT == NIT);
            if (institucion == null) return new VoidResponse($"La institución con NIT: {NIT} no se encontró", false);

            List<Docente> entities = new List<Docente>(request.Count);
            foreach (var docente in request)
            {
                if (_repository.Count(x => x.Persona.Documento.NumeroDocumento == docente.NumeroCedula && x.Persona.Institucion.NIT == NIT) > 0)
                {
                    return new VoidResponse(
                        mensaje: $"El docente con cédula {docente.NumeroCedula} ya existe para la institucion con NIT: {NIT}",
                        estado: false
                    );
                }
                else
                {
                    Docente entity = docente.ToEntity().ReverseMap();
                    entity.Persona.Institucion = institucion;
                    entity.Persona.Usuario = new UsuarioService(_unitOfWork).GenerateUser(
                        username: entity.Persona.Documento.NumeroDocumento,
                        password: "solumaticasgrm",
                        email: docente.Email,
                        tipo: TipoUsuario.Docente,
                        rol: _unitOfWork.RolRepository.FindFirstOrDefault(x => x.Nombre == TipoUsuario.Docente.ToString())
                    );
                    entities.Add(entity);
                }
            }

            _repository.AddRange(entities);
            _unitOfWork.Commit();

            return new Response<DocenteModel>(
                mensaje: $"{entities.Count} docentes agregados a la institución con NIT: {NIT}",
                data: DocenteModel.ListToModels(entities),
                estado: true
            );
        }
        public DocenteModel GetById(int key)
        {
            Docente docente = _repository.FindBy(x => x.Id == BaseModel.GetId(key), includeProperties: "Persona,Persona.Documento").FirstOrDefault();
            if (docente == null) return null;
            return new DocenteModel(docente);
        }
    }
}