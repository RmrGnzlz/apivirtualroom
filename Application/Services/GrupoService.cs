using System.Collections.Generic;
using System.Linq;
using Application.Base;
using Application.HttpModel;
using Application.Models;
using Domain.Contracts;
using Domain.Entities;

namespace Application.Services
{
    public class GrupoService : Service<Grupo>
    {
        public GrupoService(IUnitOfWork unitOfWork) : base(unitOfWork, unitOfWork.GrupoRepository)
        {
        }

        public BaseResponse AddRange(List<GrupoRequest> requests, string NIT)
        {
            Institucion institucion = _unitOfWork.InstitucionRepository.FindFirstOrDefault(x => x.NIT == NIT);
            if (institucion == null) return new VoidResponse($"La institución con NIT: {NIT} no se encontró", false);

            foreach (var request in requests)
            {
                var response = Add(request, NIT);
                if (response.Estado == false) return response;
            }

            return new VoidResponse(
                mensaje: $"Grupos agregados a la institución con NIT: {NIT}",
                estado: true
            );
        }

        public BaseResponse Add(GrupoRequest request, string NIT)
        {
            request.CedulaDocente = request.CedulaDocente.Trim().ToUpper();
            Docente docente = _unitOfWork.DocenteRepository.FindFirstOrDefault(x => x.Persona.Documento.NumeroDocumento == request.CedulaDocente && x.Persona.Institucion.NIT == NIT);
            if (docente == null)
            {
                return new VoidResponse($"El docente con documento {request.CedulaDocente} no se encontró para institución con NIT {NIT}", false);
            }
            var list = new List<string>(request.Asignaturas.Count);
            request.Asignaturas.ForEach(x =>
            {
                list.Add(x.Trim().ToUpper());
            });
            request.Asignaturas = list;
            List<Asignatura> asignaturas = _unitOfWork.AsignaturaRepository.FindBy(x => request.Asignaturas.Contains(x.Nombre) && x.Institucion.NIT == NIT, true).ToList();

            foreach (var nombreGrado in request.Grados)
            {
                if (request.Asignaturas.Contains("TODAS"))
                {
                    Grado grado = _unitOfWork.GradoRepository.FindBy(x => x.Nombre == nombreGrado, includeProperties: "GradoAsignaturas,GradoAsignaturas.Asignatura").FirstOrDefault();
                    if (grado == null) return new VoidResponse($"Grado {nombreGrado} no encontrado para institución con NIT {NIT}", false);
                    asignaturas = grado.Asignaturas();
                }
                else
                {
                    if (request.Asignaturas.Count() != asignaturas.Count)
                    {
                        string error = string.Join(';', asignaturas);
                        error += " - ";
                        error += string.Join(';', request.Asignaturas);
                        return new VoidResponse($"Algunas asignaturas ({error}) no fueron encontradas para institución con NIT {NIT}", false);
                    }
                }
                asignaturas.ForEach(x => x.GradoAsignaturas = null);
                foreach (var nombreGrupo in request.Grupos)
                {
                    Grupo grupo = SaveIfNoExist(nombreGrado, nombreGrupo);
                    if (grupo == null)
                    {
                        return new VoidResponse($"Error al guardar grupo {nombreGrupo} en el grado {nombreGrado} para institución con NIT {NIT}", false);
                    }
                    grupo.AddAsignaturas(asignaturas, docente);
                    _unitOfWork.Commit();
                    _repository.Detach(grupo);
                }
            }
            _unitOfWork.DocenteRepository.Detach(docente);
            asignaturas.ForEach(x => _unitOfWork.AsignaturaRepository.Detach(x));

            return new VoidResponse(
                mensaje: "Grupos agregados",
                estado: true
            );
        }

        public Grupo SaveIfNoExist(string nombreGrado, string nombreGrupo)
        {
            Grado grado = _unitOfWork.GradoRepository.FindFirstOrDefault(x => x.Nombre == nombreGrado);
            if (grado == null)
            {
                grado = new Grado { Nombre = nombreGrado };
                _unitOfWork.GradoRepository.Add(grado);
            }

            if (_repository.Count(x => x.Nombre == nombreGrupo && x.Grado.Nombre == nombreGrado) < 1)
            {
                _repository.Add(new Grupo { Nombre = nombreGrupo, Grado = grado });
            }
            _unitOfWork.Commit();
            _unitOfWork.GradoRepository.Detach(grado);
            return _repository.FindFirstOrDefault(x => x.Grado.Nombre == nombreGrado && x.Nombre == nombreGrupo);
        }
    }
}