using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using EnglishTutor.Common.Interfaces;
using EnglishTutor.Common.Dto;
using System.Linq;
using AutoMapper;
using EnglishTutor.Api.Models;

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
            , ISearchImageService searchImageService
            , IMapper mapper) : base(mapper)
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
                .OrderBy(s=>s.Timestamp)
                .Select(s => s.Name)
                .ToArray();

            var wordInfo = await _firebaseService.GetWordsAsync(wordNames);
            int i = 0;

            var result = wordInfo.Select(s =>
            {
                var res = Mapper.Map<WordModel>(s);
                res.Name = wordNames[i++];

                string translation;
                if  (s.Translations != null && s.Translations.TryGetValue(leng, out translation))
                    res.Translation = translation;
                return res;
            });
          

            return GenerateJsonResult(result);
        }

        [Route("{leng}/word/{name}")]
        [HttpPost]
        public async Task<JsonResult> AddWord(string leng, string name)
        {
            var normalizedWord = await _oxfordDictionaryService.GetNormalizedWordAsync(name);

            var updateVocabularyWordTask = UpdateVocabularyWord(leng, name);
            var updateUserWordTask = UpdateUserWord(leng, name);

            Task.WaitAll(updateUserWordTask, updateVocabularyWordTask);

            return GenerateJsonResult(normalizedWord);

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
            return GenerateJsonResult(new TranslationModel
            {
                Name = name,
                Language = leng,
                Translation = translation
            });
        }

        private async Task UpdateVocabularyWord(string leng, string name)
        {
            var savedWord = (await _firebaseService.GetWordsAsync(name)).FirstOrDefault();
            
            string tarnslation;

            if (savedWord == null)
            {
                var wordTask = _oxfordDictionaryService.GetWordAsync(name);
                var imagesTask = _searchImageService.GetImages(name, 3);
                var translationTask = _translateService.Translate(leng, name);

                Task.WaitAll(translationTask, imagesTask, wordTask);

                var word = wordTask.Result;
                word.Name = name;
                word.Images = imagesTask.Result;
                word.Translations = new Dictionary<string, string>
                {
                    { leng, translationTask.Result }
                };

                await _firebaseService.UpdateWordAsync(word);
            }
            else if (!savedWord.Translations.TryGetValue(leng, out tarnslation))
            {
                var translation = await _translateService.Translate(leng, name);
                await _firebaseService.UpdateWordTranslation(name, leng, translation);
            }
        }

        private async Task UpdateUserWord(string leng, string name)
        {
            var wordStatistic = await _firebaseService.GetWordStatisticAsync(UserId, name);

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
                    Name = name,
                    AddAmount = 1,
                    LastAdded = timestamp,
                    Timestamp = timestamp,
                    TaranslateAmount = 0,
                    LastTranslated = 0
                };
            }

            await _firebaseService.UpdateWordStatisticAsync(UserId, wordStatistic);
        }
    }
}
