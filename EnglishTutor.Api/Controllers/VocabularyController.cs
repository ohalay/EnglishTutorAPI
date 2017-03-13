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
        private IFirebaseService _firebaseService;
        private IOxforDictionaryService _oxfordDictionaryService;

        public VocabularyController(IFirebaseService firbaseService, IOxforDictionaryService oxfordDictionaryService)
        {
            _firebaseService = firbaseService;
            _oxfordDictionaryService = oxfordDictionaryService;
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
    }
}
