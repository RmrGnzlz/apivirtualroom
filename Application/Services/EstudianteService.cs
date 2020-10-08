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

        public BaseResponse Post(EstudianteRequest request, string NIT)
        {
            Sede sede = _unitOfWork.SedeRepository.FindFirstOrDefault(x => x.Nombre == request.Sede && x.Institucion.NIT == NIT);
            if (sede == null)
            {
                return new VoidResponse($"La sede {request.Sede} no fue encontrada", false);
            }

            Grupo grupo = _unitOfWork.GrupoRepository.FindBy(x => x.Nombre == request.Grupo && x.Grado.Nombre == request.Grado && x.Sede.Id == sede.Id, trackable: true).FirstOrDefault();

            if (grupo == null)
            {
                return new VoidResponse($"El grupo {request.Grado}-{request.Grupo} no existe para la sede {request.Sede}", false);
            }

            Estudiante estudiante = request.ToEntity().ReverseMap();
            estudiante.Sede = sede;
            grupo.AddEstudiante(estudiante);
            _unitOfWork.Commit();
            var registroUsuario = usuarioService.Add(new UsuarioModel
            {
                Username = estudiante.Persona.Documento.NumeroDocumento,
                Password = estudiante.Persona.Documento.NumeroDocumento,
                Tipo = TipoUsuarioModel.Estudiante,
            }.Include(estudiante.Persona));

            if (registroUsuario.Estado == false)
            {
                return registroUsuario;
            }

            if (_repository.Count(x => x.Id == estudiante.Id) == 0)
            {
                return new VoidResponse($"El estudiante no fue persistido en la base de datos, intente nuevamente", false);
            }

            return new Response<EstudianteModel>($"Estudiante {request.NumeroDocumento} registrado correctamente en el grupo {request.Grupo}", new EstudianteModel(estudiante), true);
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