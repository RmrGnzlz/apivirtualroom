using System;
using System.Linq;
using Application.Base;
using Application.HttpModel;
using Application.Models;
using Domain.Contracts;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Utils.ExternalServices;

namespace Application.Services
{
    public class UsuarioService : Service<Usuario>
    {
        public UsuarioService(IUnitOfWork unitOfWork) : base(unitOfWork, unitOfWork.UsuarioRepository)
        {
        }
        public BaseResponse Add(UsuarioModel request)
        {
            if (_repository.Count(x => x.Username == request.Username) > 0)
            {
                return new VoidResponse($"El usuario {request.Username} ya está registrado", false);
            }
            Persona persona = _unitOfWork.PersonaRepository.FindFirstOrDefault(x => x.Documento.NumeroDocumento == request.Persona.Documento.Numero);
            if (persona == null)
            {
                return new VoidResponse($"La persona {request.Persona.Documento.Numero} no está registrada", false);
            }

            if (_unitOfWork.RolRepository.Count(x => x.Nombre == request.Tipo.ToString()) < 1)
            {
                return new VoidResponse($"El rol no existe", false);
            }

            Usuario entity = request.ReverseMap();
            entity.Persona = persona;
            entity.Rol = _unitOfWork.RolRepository.FindFirstOrDefault(x => x.Nombre == request.Tipo.ToString());
            entity.Password = new PasswordHasher<Usuario>().HashPassword(entity, request.Password);
            _repository.Add(entity);

            if (_unitOfWork.Commit() < 1) return new VoidResponse($"No se pudo registrar el usuario {request.Username}", false);

            return new UsuarioResponse($"Usuario {request.Username} registrado correctamente", new UsuarioModel(entity).Include(entity.Rol), true);
        }
        public UsuarioResponse Auth(LoginRequest request)
        {
            Usuario usuario = _repository.FindBy(x => x.Username == request.Username, includeProperties: "Rol").FirstOrDefault();

            if (usuario == null)
            {
                return new UsuarioResponse($"No existe el usuario {request.Username}");
            }

            return ValidarPassword(usuario, request.Password) ?
                new UsuarioResponse("Crendenciales correctas", new UsuarioModel(usuario).Include(usuario.Rol), true) :
                new UsuarioResponse("Credenciales incorrectas", false);
        }
        public BaseResponse RecoverPassword(RecoverPasswordRequest request, IEmailService emailService)
        {
            Usuario usuario = _repository.FindFirstOrDefault(x => x.Username == request.Username);
            if (usuario == null)
            {
                return new VoidResponse($"El usuario {request.Username} no se encontró registrado", false);
            }
            string recoveryCode = CodigoAleatorio(6);
            string respuestaEmail = emailService.EnviarCorreo(request.Email, "Recuperación de Credenciales @SolumaticasGRM", $"El código para {request.Username} es: <br><strong>{recoveryCode}</strong>");
            if (respuestaEmail != "OK")
            {
                return new VoidResponse(respuestaEmail, false);
            }
            usuario.CodigoRecuperacion = recoveryCode;
            usuario.ExpiracionCodigo = DateTime.UtcNow.AddDays(1);
            _unitOfWork.Commit();
            return new VoidResponse($"Código de recuperación generado para: {request.Username}", true);
        }
        public UsuarioResponse ValidateRecoveryCode(RecoveryCodeRequest request)
        {
            Usuario usuario = _repository.FindFirstOrDefault(x => x.Username == request.Username && x.CodigoRecuperacion == request.RecoveryCode);
            if (usuario == null)
            {
                return new UsuarioResponse($"Código no válido para el usuario {request.Username}", false);
            }
            if (DateTime.UtcNow > usuario.ExpiracionCodigo)
            {
                return new UsuarioResponse($"El código ha expirado", false);
            }
            return new UsuarioResponse($"Código validado correctamente", new UsuarioModel(usuario), true);
        }
        public UsuarioResponse GetUsuario(string username)
        {
            Usuario usuario = _repository.FindFirstOrDefault(x => x.Username == username);
            if (usuario == null)
            {
                return new UsuarioResponse($"Usuario {username} no encontrado", false);
            }
            return new UsuarioResponse($"Usuario consultado correctamente", new UsuarioModel(usuario), true);
        }
        private static string CodigoAleatorio(byte size)
        {
            Random random = new Random(System.DateTime.UtcNow.Millisecond);
            string codigo = string.Empty;
            for (int i = 0; i < size; i++)
            {
                codigo += random.Next(0, 10).ToString();
            }
            return codigo;
        }
        public BaseResponse ChangePassword(ChangePasswordRequest request)
        {
            Usuario usuario = _repository.FindFirstOrDefault(x => x.Username == request.Username);
            if (usuario == null)
            {
                return new VoidResponse($"El usuario {request.Username} no se encontró registrado", false);
            }
            usuario.RememberPassword = usuario.Password;
            usuario.Password = new PasswordHasher<Usuario>().HashPassword(usuario, request.NewPassword);
            _unitOfWork.Commit();

            return new VoidResponse($"Password modificada con éxito para el usuario: {request.Username}", true);
        }
        public PersonaModel GetPersona(string username)
        {
            Persona persona = _unitOfWork.PersonaRepository.FindBy(x => x.Usuario.Username == username, includeProperties: "Institucion,Documento").FirstOrDefault();

            if (persona == null)
            {
                return null;
            }
            return new PersonaModel(persona).Include(persona.Institucion);
        }
        private bool ValidarPassword(Usuario usuario, string password)
        {
            var verifier = new PasswordHasher<Usuario>().VerifyHashedPassword(usuario, usuario.Password, password);
            return verifier == PasswordVerificationResult.Success;
        }
    }
}