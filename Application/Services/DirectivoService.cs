using System;
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