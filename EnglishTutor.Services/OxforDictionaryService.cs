using EnglishTutor.Common.Interfaces;
using System;
using System.Collections.Generic;
using EnglishTutor.Common.Dto;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using System.Net.Http;
using EnglishTutor.Common.AppSettings;
using EnglishTutor.Services.JsonConverters;

namespace EnglishTutor.Services
{
    public class OxforDictionaryService : BaseService, IOxforDictionaryService
    {
        private const string LANG = "en";

        private readonly OxforDictionary _oxfordDictionarySettings;

        public OxforDictionaryService(IOptions<OxforDictionary> optionOxforDictionary)
        {
            _oxfordDictionarySettings = optionOxforDictionary.Value;
        }
        protected override Uri BaseUrl => _oxfordDictionarySettings.BaseUrl;

        protected override IEnumerable<Tuple<string, string>> GetCustomHeaders()
        {
            return new List<Tuple<string, string>>
            {
                new Tuple<string, string>("app_id", _oxfordDictionarySettings.AppId),
                new Tuple<string, string>("app_key", _oxfordDictionarySettings.AppKey),
            };
        }

        public async Task<string> GetNormalizedWordAsync(string name)
        {
            return await SendRequest<string>(HttpMethod.Get
                , $"search/{LANG}?q={name}&prefix=false&limit=2&offset=0"
                , null
                ,new JPathConverter("results[0].word"));
        }

        public async Task<Word> GetWordAsync(string name)
        {
            return await SendRequest<Word>(HttpMethod.Get
                 , $"entries/{LANG}/{name}"
                 , null
                 , new WordConverter());
        }
    }
}
