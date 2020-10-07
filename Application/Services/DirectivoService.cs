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
    public class DirectivoService : Service<Directivo>
    {
        public DirectivoService(IUnitOfWork unitOfWork) : base(unitOfWork, unitOfWork.DirectivoRepository)
        {
        }

        public DirectivoResponse Add(DirectivoRequest request)
        {
            Directivo directivo = request.ToEntity().ReverseMap();
            directivo.Id = 0;
            _repository.Add(directivo);
            _unitOfWork.Commit();
            return new DirectivoResponse(
                mensaje: $"Directivo identificado con C.C {request.NumeroCedula} registrado exitosamente", 
                entidad: new Models.DirectivoModel(directivo),
                estado: true
            );
        }
        public BaseResponse AddRange(List<DirectivoRequest> request, string NIT)
        {
            Institucion institucion = _unitOfWork.InstitucionRepository.FindFirstOrDefault(x => x.NIT == NIT);
            if (institucion == null)
            {
                return new VoidResponse(
                    mensaje: $"Institución con NIT {NIT} no existe.",
                    estado: false
                );
            }
            List<Directivo> directivos = new List<Directivo>();
            foreach (var directivo in request)
            {
                Sede sede = _unitOfWork.SedeRepository.FindFirstOrDefault(x => x.Nombre == directivo.Sede && x.Institucion.NIT == NIT);
                if (sede == null)
                {
                    return new VoidResponse(
                        mensaje: $"Sede con nombre {directivo.Sede} no fue encontrada en institucion con NIT {NIT}",
                        estado: false
                    );
                }
                Directivo entity = directivo.ToEntity().ReverseMap();
                entity.Persona.Usuario = new UsuarioService(_unitOfWork).GenerateUser(
                    username: entity.Persona.Documento.NumeroDocumento,
                    password: "solumaticasgrm",
                    email: directivo.Email,
                    tipo: TipoUsuario.Directivo,
                    rol: _unitOfWork.RolRepository.FindFirstOrDefault(x => x.Nombre == TipoUsuario.Directivo.ToString())
                );
                directivos.Add(entity);
            }
            _repository.AddRange(directivos);
            _unitOfWork.Commit();

            List<DirectivoModel> entidadesRegistradas = new List<DirectivoModel>();
            directivos.ForEach(x => {
                entidadesRegistradas.Add(new DirectivoModel(x).Include(x.Persona));
            });

            return new DirectivoResponse(
                mensaje: $"{request.Count} directivos agregados con éxito",
                entidades: entidadesRegistradas,
                estado: true
            );
        }
        public DirectivoResponse AddRector(DirectivoRequest rector)
        {
            rector.Cargo = "RECTOR";
            return this.Add(rector);
        }
        public LoginResponse DatosLogin(string username)
        {
            Directivo directivo = _repository.FindBy(x => x.Persona.Usuario.Username == username, includeProperties: "Persona,Persona.Institucion,Persona.Usuario", trackable: false).FirstOrDefault();
            if (directivo == null)
            {
                return new LoginResponse($"No existe el directivo con usuario: {username}", null, false);
            }
            if (directivo.Persona == null)
            {
                return new LoginResponse($"Error al loguear directivo: {username}", null, false);
            }
            return new LoginResponse($"directivo logueado: {username}", new LoginModel
            {
                FirstPassword = string.IsNullOrEmpty(directivo.Persona.Usuario.RememberPassword),
                Nombre = $"{directivo.Persona.Nombres} {directivo.Persona.Apellidos}",
                Rol = TipoUsuarioModel.Directivo.ToString(),
                NombreInstitucion = directivo.Persona.Institucion.Nombre,
                NITInstitucion = directivo.Persona.Institucion.NIT
            }, true);
        }
    }
}