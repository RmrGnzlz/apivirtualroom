using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Base;
using Application.HttpModel;
using Application.Models;
using Application.Services;
using Domain.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace WebApi.Controllers
{
    [ApiController, Authorize]
    [Route("api/[controller]")]
    public class DocenteController : ControllerBase
    {
        readonly IUnitOfWork _unitOfWork;
        readonly DocenteService _service;
        readonly AsignaturaService _asignaturaService;
        readonly UsuarioService _usuarioService;
        public DocenteController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _service = new DocenteService(_unitOfWork);
            _asignaturaService = new AsignaturaService(_unitOfWork);
            _usuarioService = new UsuarioService(_unitOfWork);
        }

        [HttpPost, AllowAnonymous]
        public ActionResult<BaseResponse> Post(DocenteRequest request)
        {
            return Ok(_service.Post(request));
        }

        [HttpGet("MisAsignaturas"), Authorize(Roles = "Docente")]
        public ActionResult<BaseResponse> MisAsignaturas()
        {
            PersonaModel persona = _usuarioService.GetPersona(User.Identity.Name);
            var response = _asignaturaService.AsignaturasPorDocente(persona.Documento.Numero);
            if (response.Estado == false)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }
        [HttpGet("All"), Authorize(Roles = "Directivo")]
        public ActionResult<BaseResponse> Get()
        {
            PersonaModel persona = _usuarioService.GetPersona(User.Identity.Name);
            if (persona.Institucion == null)
            {
                return BadRequest(new VoidResponse($"El usuario logueado no tiene una institución asignada", true));
            }
            return Ok(_service.GetAll(persona.Institucion.Key));
        }
    }
}