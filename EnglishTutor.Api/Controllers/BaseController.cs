using EnglishTutor.Api.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EnglishTutor.Api.Controllers
{
    public class BaseController : Controller
    {
        protected ResponseModel<T> GenerateResult<T>(IEnumerable<T> input)
        {
            var list = input.ToList();

            return new ResponseModel<T>()
            {
                Result = list,
                Total = list.Count
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

        protected JsonResult GenerateJsonResult<T>(T result)
        {
                var jsonResult = new JsonResult(GenerateResult(result));
                return jsonResult;
        }
    }
}
