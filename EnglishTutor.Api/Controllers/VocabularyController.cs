using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using EnglishTutor.Common.Interfaces;
using EnglishTutor.Common.Dto;
using System.Linq;

namespace EnglishTutor.Api.Controllers
{
    public class VocabularyController : BaseController
    {
        private readonly IFirebaseService _firebaseService;
        private readonly IOxforDictionaryService _oxfordDictionaryService;
        private readonly ITranslateService _translateService;
        private readonly ISearchImageService _searchImageService;

        public VocabularyController(IFirebaseService firbaseService
            , IOxforDictionaryService oxfordDictionaryService
            , ITranslateService translateService
            , ISearchImageService searchImageService)
        {
            _firebaseService = firbaseService;
            _oxfordDictionaryService = oxfordDictionaryService;
            _translateService = translateService;
            _searchImageService = searchImageService;
        }

        [Route("{leng}/words")]
        [HttpGet]
        public async Task<JsonResult> GetLastWordsAsync(string leng, int? limitTo = 6)
        {
            var wordStatistics = await _firebaseService.GetStatisticsAsync(UserId, limitTo);

            var wordNames = wordStatistics
                .Select(s => s.Name)
                .ToArray();

            var wordInfo = await _firebaseService.GetWordsAsync(wordNames);

            return GenerateJsonResult(wordInfo);
        }

        [Route("{leng}/word/{name}")]
        [HttpPost]
        public async Task<JsonResult> AddWord(string leng, string name)
        {
            var normalizedWord = await _oxfordDictionaryService.GetNormalizedWordAsync(name);
            var wordStatistic = await _firebaseService.GetWordStatisticAsync(UserId, normalizedWord);

            var tasks = new List<Task>(2);

            var timestamp = DateTime.UtcNow.Ticks;
            if (wordStatistic != null)
            {
                ++wordStatistic.AddAmount;
                wordStatistic.LastAdded = timestamp;
            }
            else
            {
                wordStatistic = new Statistic
                {
                    Name = normalizedWord,
                    AddAmount = 1,
                    LastAdded = timestamp,
                    Timestamp = timestamp,
                    TaranslateAmount = 0,
                    LastTranslated = 0
                };

                var updateWordTask = UpdateWord(leng, normalizedWord);
                tasks.Add(updateWordTask);
            }

            var updateStatisticTask = _firebaseService.UpdateWordStatisticAsync(UserId, wordStatistic);
            tasks.Add(updateStatisticTask);

            Task.WaitAll(tasks.ToArray());

            return GenerateJsonResult(normalizedWord);

        }

        private async Task UpdateWord(string leng, string name)
        {
            var imagesTask = _searchImageService.GetImages(name, 3);
            var translationTask = _translateService.Translate(leng, name);
            var word = await _oxfordDictionaryService.GetWordAsync(name);

            word.Name = name;

            Task.WaitAll(translationTask, imagesTask);
            word.Images = imagesTask.Result;
            word.Translations.Add(leng, translationTask.Result);

            await _firebaseService.UpdateWordAsync(word);
        }

        [Route("{leng}/word/{name}/translate")]
        [HttpGet]
        public async Task<JsonResult> Translate(string leng, string name)
        {
            var translationTask = _translateService.Translate(leng, name);

            var wordStatistic = await _firebaseService.GetWordStatisticAsync(UserId, name);

            wordStatistic.Name = name;
            wordStatistic.LastTranslated = DateTime.UtcNow.Ticks;
            ++wordStatistic.TaranslateAmount;

            var updateWordTask = _firebaseService.UpdateWordStatisticAsync(UserId, wordStatistic);

            var translation = await translationTask;

            var updateTranslationTask = _firebaseService.UpdateWordTranslation(name, leng, translation);

            Task.WaitAll(updateTranslationTask, updateWordTask);
            return GenerateJsonResult(translation);
        }
    }
}
