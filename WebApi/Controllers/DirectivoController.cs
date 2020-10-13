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
    public class DirectivoController : ControllerBase
    {
        readonly IUnitOfWork _unitOfWork;
        readonly DirectivoService _service;
        public DirectivoController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _service = new DirectivoService(_unitOfWork);
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult<DirectivoResponse> Post(RegistrarDirectivosRequest request)
        {
            var response = _service.AddRange(request.Directivos, request.NIT);
            if (response.Estado == false) return BadRequest(response);
            return Ok(response);
        }
    }
}