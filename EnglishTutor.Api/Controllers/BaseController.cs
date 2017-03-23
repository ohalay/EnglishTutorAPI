using EnglishTutor.Api.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;

namespace EnglishTutor.Api.Controllers
{
    [Route("api/v1/[controller]")]
    public class BaseController : Controller
    {
        public BaseController(IMapper mapper)
        {
            Mapper = mapper;
        }
        protected ResponseModel<T> GenerateResult<T>(IEnumerable<T> input)
        {
            var list = input.ToList();

            return new ResponseModel<T>
            {
                Result = list,
                Total = list.Count
            };
        }

        protected T GenerateResult<T>(T input)
        {
            return input;
        }

        protected string UserId => User.Identity.Name;

        protected IMapper Mapper { get; }

        protected JsonResult GenerateJsonResult<T>(T result)
        {
            var jsonResult = new JsonResult(GenerateResult(result));
            return jsonResult;
        }
    }
}
