using System.Threading.Tasks;
using AutoMapper;
using EnglishTutor.Common.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace EnglishTutor.Api.Controllers
{
    public class InformationController : BaseController
    {
        private readonly ITranslateService _translateService;

        public InformationController(IMapper mapper, ITranslateService translateService) : base(mapper)
        {
            _translateService = translateService;
        }

        [Route("languages")]
        [HttpGet]
        public async Task<JsonResult> GetSupportedLanguages()
        {
            var result = await _translateService.GetSupportedLanguages();
            return GenerateJsonResult(result);
        }

    }
}