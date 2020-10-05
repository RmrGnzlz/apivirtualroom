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
    public class BaseController : ControllerBase
    {
        public static DateTime ObtenerInicioSemana()
        {
            DateTime today = DateTime.Today;
            return today.AddDays(-(byte)today.DayOfWeek);
        }
    }
}