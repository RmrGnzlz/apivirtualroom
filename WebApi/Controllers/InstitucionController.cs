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
    public class InstitucionController : ControllerBase
    {
        readonly IUnitOfWork _unitOfWork;
        readonly InstitucionService _service;
        public InstitucionController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _service = new InstitucionService(_unitOfWork);
        }

        [HttpGet("{page}/{size}")]
        public ActionResult<InstitucionResponse> GetAll(uint page = 0, uint size = 10)
        {
            var response = _service.GetAll(page: page, size: size);
            return Ok(response);
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult<InstitucionResponse> Post(InstitucionRequest request)
        {
            var response = _service.Add(request);
            if (response.Estado == false) return BadRequest(response);
            return Ok(response);
        }

        [HttpGet("buscar/{busqueda}")]
        public ActionResult<InstitucionResponse> Search(string busqueda)
        {
            var response = _service.Get(busqueda);
            return Ok(response);
        }
    }
}