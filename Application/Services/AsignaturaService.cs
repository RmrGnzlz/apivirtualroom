using System;
using System.Collections.Generic;
using System.Linq;
using Application.Base;
using Application.HttpModel;
using Application.Models;
using Domain.Contracts;
using Domain.Entities;

namespace Application.Services
{
    public class AsignaturaService : Service<Asignatura>
    {
        private GradoService _gradoService;
        public AsignaturaService(IUnitOfWork unitOfWork) : base(unitOfWork, unitOfWork.AsignaturaRepository)
        {
            _gradoService = new GradoService(unitOfWork);
        }
        public BaseResponse Get(int institucion, uint pagina, uint cantidad)
        {
            var asignaturas = base.Get(x => x.Institucion.Id == institucion, include: "", page: pagina, size: cantidad);
            return new Response<AsignaturaModel>("Asignaturas consultadas", AsignaturaModel.ListToModels(asignaturas), true);
        }

        public BaseResponse AddRange(List<AsignaturaRequest> requests, string NIT)
        {
            Institucion institucion = _unitOfWork.InstitucionRepository.FindFirstOrDefault(x => x.NIT == NIT);
            if (institucion == null) return new VoidResponse($"La institución con NIT: {NIT} no se encontró", false);

            List<Asignatura> entities = new List<Asignatura>();

            requests.ForEach(request =>
            {
                _gradoService.AddRange(request.Grados);
            });

            foreach (var request in requests)
            {
                request.Nombre = request.Nombre.Trim().ToUpper();
                Asignatura asignatura = entities.FirstOrDefault(x => x.Nombre == request.Nombre);
                if (asignatura == null)
                {
                    asignatura = _repository.FindFirstOrDefault(x => x.Nombre == request.Nombre && x.Institucion.NIT == NIT);
                    if (asignatura == null)
                    {
                        asignatura = new Asignatura
                        {
                            Nombre = request.Nombre,
                            Institucion = institucion
                        };
                        entities.Add(asignatura);
                    }
                }
                var grados = new List<Grado>();

                foreach (var y in request.Grados)
                {
                    grados.Add(_unitOfWork.GradoRepository.FindFirstOrDefault(x => x.Nombre == y.Trim().ToUpper()));
                }
                asignatura.AgregarGrados(grados);
                entities.Add(asignatura);
            }
            _repository.AddRange(entities);
            _unitOfWork.Commit();

            return new Response<AsignaturaModel>(
                mensaje: $"{entities.Count} asignaturas agregadas a la institución con NIT: {NIT}",
                data: AsignaturaModel.ListToModels(entities),
                estado: true
            );
        }

        public BaseResponse AsignaturasPorEstudiante(string username)
        {
            var estudiante = _unitOfWork.EstudianteRepository.FindBy(x => x.Persona.Usuario.Username == username, includeProperties: "Grupo", trackable: false).FirstOrDefault();
            if (estudiante == null)
            {
                return new VoidResponse($"El estudiante con usuario: {username} no está registrado", false);
            }
            else
            {
                if (estudiante.Grupo == null)
                {
                    return new VoidResponse($"El estudiante con usuario: {username} no se encuentra asignado a un grupo", false);
                }
            }
            List<GrupoAsignatura> asignaturas = AsignaturasPorGrupo(estudiante.Grupo.Id);
            string mensaje = asignaturas.Any() ? $"Asignaturas consultadas [{asignaturas.Count}]" : $"El usuario no está registrado en ninguna asignatura";
            List<GrupoAsignaturaModel> models = new List<GrupoAsignaturaModel>(asignaturas.Capacity);

            foreach (var grupoAsignatura in asignaturas)
            {
                var model = new GrupoAsignaturaModel(grupoAsignatura)
                .Include(grupoAsignatura.Asignatura)
                .Include(grupoAsignatura.Docente);
                models.Add(model);
            }
            return new Response<GrupoAsignaturaModel>($"Materias del estudiante ({models.Count})", models, true);
        }
        public BaseResponse AsignaturasPorDocente(string numeroDocumento)
        {
            var grupoAsignaturas = _unitOfWork.GrupoAsignaturaRepository.FindBy(x => x.Docente.Persona.Documento.NumeroDocumento == numeroDocumento, includeProperties: "Grupo,Asignatura,Grupo.Grado").ToList();
            if (grupoAsignaturas == null)
            {
                return new VoidResponse($"El docente no tiene asignaturas asignadas", false);
            }

            List<GrupoAsignaturaModel> models = new List<GrupoAsignaturaModel>(grupoAsignaturas.Capacity);
            foreach (var item in grupoAsignaturas)
            {
                var model = new GrupoAsignaturaModel(item)
                .Include(item.Asignatura);
                model.Grupo = new GrupoModel(item.Grupo).Include(item.Grupo.Grado);
                models.Add(model);
            }
            return new AsignaturaDocenteResponse($"Materias del docente ({models.Count})", true, models);
        }
        private List<GrupoAsignatura> AsignaturasPorGrupo(int grupoId)
        {
            var grupoAsignaturas = _unitOfWork.GrupoAsignaturaRepository.FindBy(x => x.Grupo.Id == grupoId, includeProperties: "Asignatura,Docente,Docente.Persona", trackable: false);

            if (grupoAsignaturas == null)
            {
                throw new Exception("Error al consultar el grupo");
            }
            return grupoAsignaturas.ToList();
        }
        public BaseResponse HorarioDeAsignatura(int asignaturaKey, string documentoEstudiante)
        {
            var estudiante = _unitOfWork.EstudianteRepository.FindBy(x => x.Persona.Documento.NumeroDocumento == documentoEstudiante, includeProperties: "Grupo").FirstOrDefault();
            if (estudiante == null) return new VoidResponse($"El estudiante con documento: {documentoEstudiante} no existe", false);

            GrupoAsignatura grupoAsignatura = _unitOfWork.GrupoAsignaturaRepository.FindBy(x => x.Grupo.Id == estudiante.Grupo.Id, includeProperties: "Asignatura,Horarios,GrupoAsignaturaClases,GrupoAsignaturaClases.Clase.Horario,GrupoAsignaturaClases.Clase.Multimedias").FirstOrDefault();
            if (grupoAsignatura == null)
            {
                return new VoidResponse($"El estudiante no tiene esta asignatura asignada", false);
            }
            GrupoAsignaturaModel grupoAsignaturaModel = new GrupoAsignaturaModel(grupoAsignatura)
            .Include(grupoAsignatura.Horarios);
            grupoAsignaturaModel.Clases = new List<ClaseModel>();

            grupoAsignatura.Clases.ForEach(x =>
            {
                grupoAsignaturaModel.Clases.Add(new ClaseModel(x).Include(x.Multimedias).Include(x.Horario));
            });
            return new Response<GrupoAsignaturaModel>($"Asignatura consultada", grupoAsignaturaModel, true);
        }
    }
}