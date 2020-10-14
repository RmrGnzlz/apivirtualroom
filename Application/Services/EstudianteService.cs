using System.Collections.Generic;
using System.Linq;
using Application.Base;
using Application.HttpModel;
using Application.Models;
using Domain.Contracts;
using Domain.Entities;

namespace Application.Services
{
    public class EstudianteService : Service<Estudiante>
    {
        UsuarioService usuarioService;
        public EstudianteService(IUnitOfWork unitOfWork) : base(unitOfWork, unitOfWork.EstudianteRepository)
        {
            usuarioService = new UsuarioService(_unitOfWork);
        }

        public BaseResponse AddRange(List<EstudianteRequest> requests, string NIT)
        {
            Institucion institucion = _unitOfWork.InstitucionRepository.FindFirstOrDefault(x => x.NIT == NIT);
            if (institucion == null) return new VoidResponse($"La institución con NIT: {NIT} no se encontró", false);
            Sede sede = _unitOfWork.SedeRepository.FindFirstOrDefault(x => x.Institucion.NIT == NIT);
            if (sede == null)
            {
                sede = new Sede
                {
                    Nombre = "Principal",
                    Direccion = institucion.Nombre,
                    Institucion = institucion
                };
                _unitOfWork.SedeRepository.Add(sede);
            }

            List<Estudiante> entities = new List<Estudiante>(requests.Count);
            foreach (var request in requests)
            {
                request.Sede = request.Sede.ToUpper();
                request.Grado = request.Grado.ToUpper();
                request.Grupo = request.Grupo.ToUpper();
                Grupo grupo = _unitOfWork.GrupoRepository.FindFirstOrDefault(x => x.Nombre == request.Grupo && x.Grado.Nombre == request.Grado, trackable: true);
                if (grupo == null)
                {
                    return new VoidResponse($"El grupo {request.Grado}-{request.Grupo} no existe para la institucion con NIT {NIT}", false);
                }
                if (_repository.Count(x => x.Persona.Documento.NumeroDocumento == request.NumeroDocumento && x.Persona.Institucion.NIT == NIT) > 0)
                {
                    return new VoidResponse(
                        mensaje: $"El estudiante con documento {request.NumeroDocumento} ya existe para la institucion con NIT: {NIT}",
                        estado: false
                    );
                }
                Estudiante entity = request.ToEntity().ReverseMap();
                entity.Sede = sede;
                entity.Persona.Institucion = institucion;
                entity.Grupo = grupo;
                entity.Persona.Usuario = new UsuarioService(_unitOfWork).GenerateUser(
                    username: entity.Persona.Documento.NumeroDocumento,
                    password: "solumaticasgrm",
                    email: request.Email,
                    tipo: TipoUsuario.Estudiante,
                    rol: _unitOfWork.RolRepository.FindFirstOrDefault(x => x.Nombre == TipoUsuario.Estudiante.ToString())
                );
                entities.Add(entity);
            }

            _repository.AddRange(entities);
            _unitOfWork.Commit();

            return new Response<EstudianteModel>($"{entities.Count} estudiantes registrados correctamente en la institucion con NIT {NIT}", EstudianteModel.ListToModels(entities), true);
        }

        public BaseResponse Get(string busqueda)
        {
            if (busqueda == string.Empty) return new VoidResponse($"Búsqueda vacía", false);

            List<EstudianteModel> estudiantes = EstudianteModel.ListToModels(_unitOfWork.EstudianteRepository.Search(busqueda).ToList());
            return new Response<EstudianteModel>($"Registros consultados [{busqueda}]", estudiantes, true);
        }
        public LoginResponse DatosLogin(string username)
        {
            Estudiante estudiante = _repository.FindBy(x => x.Persona.Usuario.Username == username, includeProperties: "Persona,Grupo,Persona.Institucion,Persona.Usuario").FirstOrDefault();
            if (estudiante == null)
            {
                return new LoginResponse($"No existe el estudiante con usuario: {username}", null, false);
            }
            if (estudiante.Persona == null)
            {
                return new LoginResponse($"No se pudo loguear el estudiante: {username}", null, false);
            }
            return new LoginResponse($"Estudiante Logueado", new LoginModel
            {
                FirstPassword = string.IsNullOrEmpty(estudiante.Persona.Usuario.RememberPassword),
                Nombre = $"{estudiante.Persona.Nombres} {estudiante.Persona.Apellidos}",
                NombreGrado = estudiante.GradoActual.ToString(),
                NombreGrupo = estudiante.Grupo.Nombre,
                NombreInstitucion = estudiante.Persona.Institucion.Nombre,
                NITInstitucion = estudiante.Persona.Institucion.NIT,
                Rol = TipoUsuarioModel.Estudiante.ToString()
            }, true);
        }
    }
}