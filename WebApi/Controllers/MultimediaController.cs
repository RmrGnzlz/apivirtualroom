using Application.HttpModel;
using Application.Services;
using Domain.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [ApiController, Authorize]
    [Route("api/[controller]")]
    public class MultimediaController : ControllerBase
    {
        readonly IUnitOfWork _unitOfWork;
        readonly MultimediaService _service;

        public MultimediaController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _service = new MultimediaService(_unitOfWork);
        }

        [HttpGet]
        public ActionResult<MultimediaResponse> GetAll(uint pagina = 0, uint cantidad = 10)
        {
            var response = _service.GetAll(page: pagina, size: cantidad);
            return Ok(response);
        }
    }
}