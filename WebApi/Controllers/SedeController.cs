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

namespace WebApi.Controllers
{
    [ApiController, Authorize]
    [Route("api/[controller]")]
    public class SedeController : ControllerBase
    {
        readonly IUnitOfWork _unitOfWork;
        readonly SedeService _service;
        public SedeController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _service = new SedeService(_unitOfWork);
        }
        [HttpPost, Authorize]
        public ActionResult<SedeResponse> Post(SedeRequest request)
        {
            var response = _service.Add(request);
            if (response.Estado == false) return BadRequest(response);
            return Ok(response);
        }

        [HttpGet("buscar/{busqueda}"), AllowAnonymous]
        public ActionResult<SedeResponse> Search(string busqueda)
        {
            var response = _service.Search(busqueda);
            return Ok(response);
        }
    }
}