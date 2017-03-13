using EnglishTutor.Common.Interfaces;
using System;
using System.Collections.Generic;
using EnglishTutor.Common.Dto;
using System.Threading.Tasks;
using EnglishTutor.Common;
using Microsoft.Extensions.Options;
using System.Net.Http;
using Newtonsoft.Json;

namespace EnglishTutor.Services
{
    public class OxforDictionaryService : BaseService, IOxforDictionaryService
    {
        private const string LANG = "en";

        private AppSettings _appSettings;

        public OxforDictionaryService(IOptions<AppSettings> optionAppSettings)
        {
            _appSettings = optionAppSettings.Value;
        }
        protected override Uri BaseUrl => _appSettings.OxforDictionary.BaseUrl;

        protected override IEnumerable<Tuple<string, string>> GetCustomHeaders()
        {
            return new List<Tuple<string, string>>
            {
                new Tuple<string, string>("app_id", _appSettings.OxforDictionary.AppId),
                new Tuple<string, string>("app_key", _appSettings.OxforDictionary.AppKey),
            };
        }

        public async Task<string> GetNormalizedWordAsync(string name)
        {
            return await SendRequest(HttpMethod.Get
                , $"search/{LANG}?q={name}&prefix=false&limit=2&offset=0"
                , null
                , async str =>
                {
                    var normalizedName = JsonConvert.DeserializeObject<string>(str, new JPathConverter("results[0].word"));
                    return await Task.FromResult(normalizedName);
                });
        }

        public async Task<Word> GetWordAsync(string name)
        {
            return await SendRequest(HttpMethod.Get
                 , $"entries/{LANG}/{name}"
                 , null
                 , async str =>
                 {
                     var normalizedName = JsonConvert.DeserializeObject<Word>(str, new WordConverter());
                     return await Task.FromResult(normalizedName);
                 });
        }
    }
}
