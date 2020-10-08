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
    public class GrupoController : ControllerBase
    {
        readonly IUnitOfWork _unitOfWork;
        readonly GrupoService _service;
        public GrupoController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _service = new GrupoService(_unitOfWork);
        }

        [HttpPost]
        public ActionResult<BaseResponse> Post(RegistroGruposRequest request)
        {
            BaseResponse response = new Response<RegistroGruposRequest>(mensaje: "Entidades recibidas correctamente", request, true);
            return Ok(response);
        }
    }
}