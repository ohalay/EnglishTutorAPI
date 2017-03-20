﻿using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using EnglishTutor.Common.Interfaces;
using EnglishTutor.Common.Dto;
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
        public async Task<JsonResult> GetLastWordsAsync(int? limitTo)
        {
            var wordStatistics = await _firebaseService.GetStatisticsAsync(UserId, limitTo);

            var wordNames = wordStatistics
                .Select(s => s.Name)
                .ToArray();

            var wordInfo = await _firebaseService.GetWordsAsync(wordNames);

            return GenerateJsonResult(wordInfo);
        }

        [Route("word/{name}")]
        [HttpPost]
        public async Task<JsonResult> AddWord(string name)
        {
            var normalizedWord = await _oxfordDictionaryService.GetNormalizedWordAsync(name);
            var wordStatistic = await _firebaseService.GetWordStatisticAsync(UserId, normalizedWord);

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

                var word = UpdateWord(normalizedWord);
            }

            var statistic = _firebaseService.UpdateWordStatisticAsync(UserId, wordStatistic);

            return GenerateJsonResult(normalizedWord);
        }

        private async Task UpdateWord(string name)
        {
            var word = await _oxfordDictionaryService.GetWordAsync(name);
            word.Name = name;
            await _firebaseService.UpdateWordAsync(word);
        }

        [Route("word/{name}/translate")]
        [HttpGet]
        public async Task<JsonResult> Translate(string to, string name)
        {
            const string FROM = "en";

            var wordStatistic = await _firebaseService.GetWordStatisticAsync(UserId, name);

            wordStatistic.LastTranslated = DateTime.UtcNow.Ticks;
            ++wordStatistic.TaranslateAmount;

            await _firebaseService.UpdateWordStatisticAsync(UserId, wordStatistic);

            var res = await _translateService.Translate(FROM, to, name);

            return GenerateJsonResult(res);
        }


    }
}
