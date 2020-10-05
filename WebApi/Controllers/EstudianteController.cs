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
    public class EstudianteController : ControllerBase
    {
        readonly IUnitOfWork _unitOfWork;
        readonly EstudianteService _service;
        public EstudianteController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _service = new EstudianteService(_unitOfWork);
        }

        [HttpPost, AllowAnonymous]
        public ActionResult<BaseResponse> Post(EstudianteRequest request)
        {
            return Ok(_service.Post(request));
        }

        [HttpGet("buscar/{busqueda}")]
        public ActionResult<BaseResponse> Get(string busqueda)
        {
            return Ok(_service.Get(busqueda));
        }
    }
}