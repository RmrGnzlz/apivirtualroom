using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Application.Base;
using Application.HttpModel;
using Application.Mapping;
using Application.Models;
using Domain.Contracts;
using Domain.Entities;

namespace Application.Services
{
    public class ClaseService : Service<Clase>
    {
        public ClaseService(IUnitOfWork unitOfWork) : base(unitOfWork, unitOfWork.ClaseRepository)
        {
        }
        public BaseResponse Post(ClaseRequest request, string documentoDocente)
        {
            // Condición con la que buscaremos los grupos-asignatura
            Expression<Func<GrupoAsignatura, bool>> condicion = x => x.Asignatura.Id == request.AsignaturaKey && x.Grupo.Grado.Nombre == request.Grado && x.Docente.Persona.Documento.NumeroDocumento == documentoDocente && x.Asignatura.Institucion.NIT == request.InstitucionNIT;
            List<GrupoAsignatura> grupos = _unitOfWork.GrupoAsignaturaRepository.FindBy(condicion, true).ToList();

            if (!grupos.Any())
            {
                return new VoidResponse($"No se encontraron grupos relacionados a la asignatura: {request.AsignaturaKey} del grado: {request.Grado}, para el docente: {documentoDocente} en la institución {request.InstitucionNIT}", false);
            }

            Clase clase = request.ToEntity().ReverseMap();
            clase.Asignatura = _unitOfWork.AsignaturaRepository.FindFirstOrDefault(x => x.Id == request.AsignaturaKey);
            request.Multimedias.ForEach(x =>
            {
                clase.AddMultimedia(x.ToEntity().ReverseMap());
            });
            grupos.ForEach(x =>
            {
                x.AddClase(clase);
            });
            int saves = _unitOfWork.Commit();
            return new Response<ClaseModel>($"{saves} datos guardados correctamente", new ClaseModel(clase).Include(clase.Multimedias), true);
        }
        public BaseResponse ClasesConMultimedia(DateTime fechaInicial, DateTime fechaFinal, string documentoEstudiante, int asignaturaKey = 0)
        {
            Estudiante estudiante = _unitOfWork.EstudianteRepository.FindBy(x => x.Persona.Documento.NumeroDocumento == documentoEstudiante, includeProperties: "Grupo").FirstOrDefault();
            if (estudiante == null) return new VoidResponse($"El estudiante con documento: {documentoEstudiante} no existe", false);
            if (estudiante.Grupo == null)
            {
                return new VoidResponse($"El estudiante no está relacionado a un grupo", false);
            }
            Expression<Func<GrupoAsignaturaClase, bool>> condicion = x => x.Clase.FechaInicio >= fechaInicial && x.Clase.FechaCierre <= fechaFinal && x.GrupoAsignatura.Grupo == estudiante.Grupo;
            if (asignaturaKey > 0)
            {
                condicion = x => x.Clase.FechaInicio >= fechaInicial && x.Clase.FechaCierre <= fechaFinal && x.GrupoAsignatura.Grupo == estudiante.Grupo && x.GrupoAsignatura.Asignatura.Id == BaseModel.GetId(asignaturaKey);
            }
            string includeProperties = "Clase.Horario,Clase.Multimedias,GrupoAsignatura,GrupoAsignatura.Grupo,GrupoAsignatura.Asignatura";
            List<ClaseModel> claseModels = BuscarClasesPor(condicion, includeProperties);
            return new Response<ClaseModel>($"Asignaturas consultadas para el estudiante {documentoEstudiante}", claseModels, true);
        }
        public BaseResponse ClasesPorDocente(DateTime fechaInicial, int docenteKey, int asignaturaKey)
        {
            string includeProperties = "Clase.Horario,Clase.Multimedias,GrupoAsignatura,GrupoAsignatura.Grupo,GrupoAsignatura.Asignatura";
            Expression<Func<GrupoAsignaturaClase, bool>> condicion = x => x.GrupoAsignatura.Docente.Id == BaseModel.GetId(docenteKey) && x.GrupoAsignatura.Asignatura.Id == BaseModel.GetId(asignaturaKey) && x.Clase.FechaInicio >= fechaInicial;
            var claseModels = BuscarClasesPor(condicion, includes: includeProperties);
            return new Response<ClaseModel>($"Asignaturas consultadas para el docente {docenteKey}", claseModels, true);
        }
        private List<ClaseModel> BuscarClasesPor(Expression<Func<GrupoAsignaturaClase, bool>> condicion, string includes)
        {
            var grupoAsignaturaClases = _unitOfWork.GrupoAsignaturaClaseRepository.FindBy(condicion, includeProperties: includes).ToList();
            var clases = new List<Clase>();
            grupoAsignaturaClases.ForEach(x =>
            {
                if (clases.Where(c => c.Id == x.ClaseId || c.Id == x.Clase.Id).Count() < 1) clases.Add(x.Clase);
            });
            List<ClaseModel> clasesModel = new List<ClaseModel>(clases.ToList().Capacity);
            foreach (var clase in clases)
            {
                ClaseModel claseModel = new ClaseModel(clase)
                .Include(clase.Horario)
                .Include(clase.Multimedias);
                claseModel.GrupoAsignaturas = new List<GrupoAsignaturaModel>();
                clase.GrupoAsignaturas.ForEach(x =>
                {
                    claseModel.GrupoAsignaturas.Add(new GrupoAsignaturaModel(x).Include(x.Asignatura));
                });
                clasesModel.Add(claseModel);
            }
            return clasesModel;
        }
        public BaseResponse ClasesPorAsignatura(int asignaturaKey)
        {
            var entities = _repository.FindBy(x => x.Asignatura.Id == asignaturaKey, includeProperties: "Asignatura,GrupoAsignaturaClases").ToList();
            var asignatura = _unitOfWork.AsignaturaRepository.FindFirstOrDefault(x => x.Id == asignaturaKey);
            return new Response<ClaseModel>($"Clases de la asignatura {asignaturaKey}: {asignatura.Nombre}", ClaseModel.ListToModels(entities), true);
        }
    }
}