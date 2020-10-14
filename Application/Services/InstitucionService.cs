using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Application.Base;
using Application.HttpModel;
using Application.Models;
using Domain.Contracts;
using Domain.Entities;

namespace Application.Services
{
    public class InstitucionService : Service<Institucion>
    {
        public InstitucionService(IUnitOfWork unitOfWork) : base(unitOfWork, unitOfWork.InstitucionRepository)
        {
        }

        public BaseResponse Add(InstitucionRequest request)
        {
            Municipio municipio = _unitOfWork.MunicipioRepository.FindFirstOrDefault(x => x.Codigo == request.CodigoMunicipio, true);

            if (municipio == null)
            {
                return new VoidResponse(
                    mensaje: $"Municipio {request.CodigoMunicipio} no encontrado",
                    estado: false
                );
            }

            // Directivo rector = _unitOfWork.DirectivoRepository.FindFirstOrDefault(x => x.Persona.Documento.NumeroDocumento == request.Rector.NumeroCedula, trackable: true);
            // if (rector == null)
            // {
            //     DirectivoService directivoService = new DirectivoService(_unitOfWork);
            //     DirectivoResponse directivoResponse = directivoService.AddRector(request.Rector);
            //     if (directivoResponse.Estado == false || !directivoResponse.Data.Any())
            //     {
            //         return new VoidResponse(
            //             mensaje: directivoResponse.Mensaje,
            //             estado: false
            //         );
            //     }
            //     rector = directivoResponse.Data.FirstOrDefault().ReverseMap();
            // }

            Institucion institucion = request.ToEntity().ReverseMap();
            institucion.Municipio = municipio;
            // institucion.Rector = rector;
            institucion.Sedes = new List<Sede>();
            foreach (var item in request.Sedes)
            {
                institucion.Sedes.Add(item.ToEntity().ReverseMap());
            }

            _repository.Add(institucion);
            _unitOfWork.Commit();

            return new InstitucionResponse(
                mensaje: $"Institucion {institucion.Nombre} agregada con  éxito",
                entidad: new InstitucionModel(institucion).Include(institucion.Municipio).Include(institucion.Sedes),
                estado: true
            );
        }

        public BaseResponse Get(string busqueda)
        {
            busqueda = busqueda.ToUpper();
            var entities = _repository.FindBy(x => x.NIT.ToUpper().Contains(busqueda) || x.Nombre.ToUpper().Contains(busqueda) || x.DANE.ToUpper().Contains(busqueda), false).ToList();
            return new InstitucionResponse($"Instituciones que coinciden con: {busqueda}", InstitucionModel.ListToModels(entities), true);
        }

        public InstitucionResponse GetAll(Expression<Func<Institucion, bool>> expression = null, uint page = 0, uint size = 10)
        {
            List<Institucion> institucions = base.Get(expression, "Municipio,Municipio.Departamento", page, size);
            List<InstitucionModel> response = new List<InstitucionModel>();

            institucions.ForEach(x => {
                response.Add(new InstitucionModel(x).Include(x.Municipio));
            });
            return new InstitucionResponse(
                "Registros consultados correctamente",
                response,
                true
            );
        }
    }
}