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
    public class AsignaturaController : ControllerBase
    {
        readonly IUnitOfWork _unitOfWork;
        readonly AsignaturaService _service;
        readonly UsuarioService _usuarioService;

        public AsignaturaController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _service = new AsignaturaService(_unitOfWork);
            _usuarioService = new UsuarioService(_unitOfWork);
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult<BaseResponse> Post(RegistroAsignaturasRequest request)
        {
            BaseResponse response = new Response<RegistroAsignaturasRequest>(mensaje: "Entidades recibidas correctamente", request, true);
            return Ok(response);
        }

        [HttpGet("asignaturas/{institucionKey}/{pagina}/{cantidad}")]
        public ActionResult<BaseResponse> GetAll(int institucionKey, uint pagina = 0, uint cantidad = 10)
        {
            return Ok(_service.Get(institucionKey, pagina, cantidad));
        }

        [HttpGet("MisAsignaturas"), Authorize(Roles = "Estudiante")]
        public ActionResult<BaseResponse> MisAsignaturas()
        {
            return Ok(_service.AsignaturasPorEstudiante(User.Identity.Name));
        }

        [HttpGet("{asignaturaKey}/Horarios")]
        public ActionResult<BaseResponse> Horarios(int asignaturaKey)
        {
            var persona = _usuarioService.GetPersona(User.Identity.Name);
            if (persona == null) return BadRequest(new VoidResponse($"Error al obtener datos personales", false));
            var response = _service.HorarioDeAsignatura(asignaturaKey, persona.Documento.Numero);
            return Ok(response);
        }

        [HttpGet("Docente/{key}")]
        public ActionResult<BaseResponse> PorDocente(int key)
        {
            var docente = new DocenteService(_unitOfWork).GetById(key);
            if (docente == null) return BadRequest(new VoidResponse($"El docente {key} no se encontró", false));
            if (docente.DatosPersonales == null) return BadRequest(new VoidResponse($"Error al obtener datos personales", false));
            if (docente.DatosPersonales.Documento == null) return BadRequest(new VoidResponse($"Error al obtener identificación del docente", false));
            var response = _service.AsignaturasPorDocente(docente.DatosPersonales.Documento.Numero);
            return Ok(response);
        }
    }
}