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

        protected async Task<JsonResult> ExecuteResult<T>(Func<Task<T>> func)
        {
            try
            {
                var res = await func();
                var jsonResult = new JsonResult(GenerateResult(res));
                return jsonResult;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}
