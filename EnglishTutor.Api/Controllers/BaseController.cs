using EnglishTutor.Api.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EnglishTutor.Api.Controllers
{
    public class BaseController : Controller
    {
        protected ResponseModel<T> GenerateResult<T>(IEnumerable<T> input)
        {
            return new ResponseModel<T>()
            {
                Result = input,
                Total = input.Count()
            };
        }

        protected ResponseModel<T> GenerateResult<T>(T input)
        {
            return new ResponseModel<T>()
            {
                Result = new List<T> { input },
            };
        }

        protected string UserId
        {
            get { return "115787596179138188666"; }
        }
    }
}
