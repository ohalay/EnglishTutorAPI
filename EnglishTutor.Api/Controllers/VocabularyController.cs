using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using EnglishTutor.Common.Interfaces;
using EnglishTutor.Common.Dto;
using EnglishTutor.Api.Models;
using System.Linq;

namespace EnglishTutor.Api.Controllers
{
    [Route("api/[controller]")]
    public class VocabularyController : BaseController
    {
        private readonly IFirebaseService _firebaseService;
        private readonly IOxforDictionaryService _oxfordDictionaryService;
        private readonly ITranslateService _translateService;

        public VocabularyController(IFirebaseService firbaseService
            , IOxforDictionaryService oxfordDictionaryService
            , ITranslateService translateService)
        {
            _firebaseService = firbaseService;
            _oxfordDictionaryService = oxfordDictionaryService;
            _translateService = translateService;
        }

        [Route("word")]
        [HttpGet]
        public async Task<ResponseModel<Word>> GetLastWordsAsync(int? limitTo)
        {
            var wordStatistics = await _firebaseService.GetStatisticAsync(UserId, limitTo);

            var wordNames = wordStatistics
                .Select(s => s.Name)
                .ToArray();

            var wordInfo = await _firebaseService.GetWordsAsync(wordNames);

            return GenerateResult(wordInfo);
        }

        [Route("word1")]
        [HttpGet]
        public async Task<JsonResult> GetLastWordsJsonAsync(int? limitTo)
        {

            var wordStatistics = await _firebaseService.GetStatisticAsync(UserId, limitTo);

            var wordNames = wordStatistics
                .Select(s => s.Name)
                .ToArray();

            var res = await _firebaseService.GetWordsAsync(wordNames);
            return GenerateJsonResult(res);
        }

        [Route("normalizedWord")]
        [HttpGet]
        public async Task<ResponseModel<string>> GetNormalizedWordAsync(string name)
        {
            var word = await _oxfordDictionaryService.GetNormalizedWordAsync(name);

            return GenerateResult(word);
        }

        [Route("odWord")]
        [HttpGet]
        public async Task<ResponseModel<Word>> GetODWordAsync(string name)
        {
            var word = await _oxfordDictionaryService.GetWordAsync(name);

            return GenerateResult(word);
        }

        [Route("translate")]
        [HttpGet]
        public async Task<ResponseModel<string>> Translate(string from, string to, string text)
        {
            var res = await _translateService.Translate(from, to , text);

            return GenerateResult(res);
        }


    }
}
