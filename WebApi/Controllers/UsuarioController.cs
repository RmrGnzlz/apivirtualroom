using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Application.Base;
using Application.HttpModel;
using Application.Models;
using Application.Services;
using Domain.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Utils.ExternalServices;
using WebApi.Models;

namespace WebApi.Controllers
{
    [ApiController, Authorize]
    [Route("api/[controller]")]
    public class UsuarioController : ControllerBase
    {
        readonly IUnitOfWork _unitOfWork;
        readonly UsuarioService _service;
        readonly IEmailService _emailService;
        private readonly IConfiguration _configuration;
        public UsuarioController(IUnitOfWork unitOfWork, IConfiguration configuration, IEmailService emailService)
        {
            _configuration = configuration;
            _unitOfWork = unitOfWork;
            _service = new UsuarioService(_unitOfWork);
            _emailService = emailService;
        }

        [HttpGet("Renew/Token")]
        public ActionResult<BaseResponse> Claims()
        {
            var response = _service.GetUsuario(User.Identity.Name);
            if (response.Estado == false)
            {
                return BadRequest(response);
            }
            return LoginResponse(response);
        }

        [HttpPost("ChangePassword")]
        public ActionResult<BaseResponse> ChangePassword(ChangePasswordRequest request)
        {
            request.Username = User.Identity.Name;
            if (string.IsNullOrEmpty(request.Username))
            {
                return BadRequest(new VoidResponse("El username es obligatorio", false));
            }
            var response = _service.ChangePassword(request);
            if (response.Estado == false)
            {
                return BadRequest(response);
            }
            return Auth(new LoginRequest { Username = request.Username, Password = request.NewPassword });
        }
        [HttpPost("RecoverPassword"), AllowAnonymous]
        public ActionResult<BaseResponse> RecoverPassword(RecoverPasswordRequest request)
        {
            var response = _service.RecoverPassword(request, _emailService);
            if (response.Estado == false)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }
        [HttpPost("ValidateCode"), AllowAnonymous]
        public ActionResult<BaseResponse> ValidateCode(RecoveryCodeRequest request)
        {
            var response = _service.ValidateRecoveryCode(request);
            if (response.Estado == false || response.Data == null)
            {
                return BadRequest(response);
            }
            return LoginResponse(response);
        }
        [AllowAnonymous, Produces("application/json")]
        [HttpPost("auth")]
        public ActionResult<BaseResponse> Auth(LoginRequest request)
        {
            UsuarioResponse response = _service.Auth(request);
            return LoginResponse(response);
        }
        private ActionResult<BaseResponse> LoginResponse(UsuarioResponse response)
        {
            if (!response.Estado || response.Data == null)
            {
                return BadRequest(new VoidResponse("Usuario y/o Contraseña incorrectos", false));
            }
            var usuario = response.Data.FirstOrDefault();
            if (usuario == null) return Unauthorized(new VoidResponse($"Error al obtener datos de usuario", false));
            var token = BuildToken(usuario);
            PersonaModel persona = _service.GetPersona(usuario.Username);

            if (persona == null)
            {
                return BadRequest(new VoidResponse($"Persona no encontrada para el usuario {usuario.Username}", false));
            }

            if (usuario.Tipo == TipoUsuarioModel.Estudiante)
            {
                LoginResponse loginResponse = new EstudianteService(_unitOfWork).DatosLogin(usuario.Username);
                if (loginResponse.Estado == false)
                {
                    return Unauthorized(loginResponse);
                }
                loginResponse.Auth = token;
                return Ok(loginResponse);
            }
            if (usuario.Tipo == TipoUsuarioModel.Docente)
            {
                LoginResponse loginResponse = new DocenteService(_unitOfWork).DatosLogin(usuario.Username);
                if (loginResponse.Estado == false)
                {
                    return Unauthorized(loginResponse);
                }
                loginResponse.Auth = token;
                return Ok(loginResponse);
            }
            if (usuario.Tipo == TipoUsuarioModel.Directivo)
            {
                LoginResponse loginResponse = new DirectivoService(_unitOfWork).DatosLogin(usuario.Username);
                if (loginResponse.Estado == false)
                {
                    return Unauthorized(loginResponse);
                }
                loginResponse.Auth = token;
                return Ok(loginResponse);
            }
            return Unauthorized(response);
        }
        private TokenJwt BuildToken(UsuarioModel user)
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.UniqueName, user.Username),
                new Claim(ClaimTypes.Role, user.Tipo.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };
            string encript = _configuration.GetSection("TOKEN").Value;
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(encript));
            var creds = new SigningCredentials(key, Microsoft.IdentityModel.Tokens.SecurityAlgorithms.HmacSha256);

            var expiration = DateTime.UtcNow.AddDays(1);

            JwtSecurityToken token = new JwtSecurityToken(
               issuer: "yourdomain.com",
               audience: "yourdomain.com",
               claims: claims,
               expires: expiration,
               signingCredentials: creds
            );

            return new TokenJwt
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                Expiracion = expiration
            };
        }
    }
}