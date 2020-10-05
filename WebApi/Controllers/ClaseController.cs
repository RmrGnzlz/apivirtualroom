using System;
using System.Collections.Generic;
using Application.Base;
using Application.HttpModel;
using Application.Models;
using Application.Services;
using Domain.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [ApiController, Authorize]
    [Route("api/[controller]")]
    public class ClaseController : ControllerBase
    {
        readonly IUnitOfWork _unitOfWork;
        readonly ClaseService _service;
        readonly UsuarioService _usuarioService;

        public ClaseController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _service = new ClaseService(_unitOfWork);
            _usuarioService = new UsuarioService(_unitOfWork);
        }
        [HttpGet("PorSemana")]
        public ActionResult<BaseResponse> PorSemana()
        {
            var persona = _usuarioService.GetPersona(User.Identity.Name);
            if (persona == null)
            {
                return Unauthorized(new VoidResponse($"Persona no encontrada", false));
            }
            DateTime inicio = BaseController.ObtenerInicioSemana();
            DateTime fin = inicio.AddDays(6);
            var response = _service.ClasesConMultimedia(inicio, fin, persona.Documento.Numero);
            return Ok(response);
        }
        [HttpGet("PorSemana/{asignatura}")]
        public ActionResult<BaseResponse> PorSemana(int asignatura)
        {
            var persona = _usuarioService.GetPersona(User.Identity.Name);
            if (persona == null)
            {
                return Unauthorized(new VoidResponse($"Persona no encontrada", false));
            }
            DateTime inicio = BaseController.ObtenerInicioSemana();
            DateTime fin = inicio.AddDays(6);
            var response = _service.ClasesConMultimedia(inicio, fin, persona.Documento.Numero, asignatura);
            return Ok(response);
        }
        [HttpGet("PorDocente/{docenteKey}/{asignaturaKey}"), Authorize(Roles = "Directivo")]
        public ActionResult<BaseResponse> PorDocente(int docenteKey, int asignaturaKey)
        {
            DateTime inicio = BaseController.ObtenerInicioSemana();
            var response = _service.ClasesPorDocente(inicio, docenteKey, asignaturaKey);
            return Ok(response);
        }
        [HttpPost]
        public ActionResult<BaseResponse> Post(ClaseRequest request)
        {
            UsuarioService usuarioService = new UsuarioService(_unitOfWork);
            PersonaModel persona = usuarioService.GetPersona(User.Identity.Name);
            if (persona == null)
            {
                return BadRequest(new VoidResponse("Error al buscar datos personales del usuario", false));
            }
            return Ok(_service.Post(request, persona.Documento.Numero));
        }
        [HttpGet("{grupoAsignaturaKey}"), Authorize(Roles = "Directivo,Admin,Docente")]
        public ActionResult<BaseResponse> PorAsignatura(int grupoAsignaturaKey)
        {
            var response = _service.ClasesPorAsignatura(grupoAsignaturaKey);
            return Ok(response);
        }
    }
}