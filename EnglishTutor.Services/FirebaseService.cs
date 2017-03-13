using EnglishTutor.Common.Interfaces;
using System;
using EnglishTutor.Common.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json;
using EnglishTutor.Common;
using Microsoft.Extensions.Options;

namespace EnglishTutor.Services
{
    public class FirebaseService : BaseService, IFirebaseService
    {
        private AppSettings _appSettings;

        public FirebaseService(IOptions<AppSettings> optionAppSettings)
        {
            _appSettings = optionAppSettings.Value;
        }

        protected override Uri BaseUrl => _appSettings.Firebase.BaseUrl;

        private async Task<Word> GetWordAsync(string name)
        {
            return await SendRequest<Word>(HttpMethod.Get, $"vocabulary/{name}.json");
        }

        public async Task<IEnumerable<Statistic>> GetStatisticAsync(string userId, int? limitTo)
        {
            return await SendRequest(HttpMethod.Get
                , $"users/{userId}/statistics.json?orderBy=\"timestamp\"&limitToLast={limitTo}"
                ,null
                , async str => 
                {
                    var list = JsonConvert.DeserializeObject<IEnumerable<Statistic>>(str, new StatisticConverter());
                    return await Task.FromResult(list);
                });
        }

        public async Task<IEnumerable<Word>> GetWordsAsync(params string[] wordNames)
        {
            var tasks = new Task<Word>[wordNames.Length];
            int i = 0;

            foreach(var name in wordNames)
            {
                tasks[i++] = GetWordAsync(name);
            }

            return await Task.WhenAll(tasks);
        }

        public async Task<Word> UpdateWordAsync(Word word)
        {
            return await SendRequest(new HttpMethod("PATCH")
               , $"vocabulary/{word.Name}.json"
               , word
               , async str => await Task.FromResult(word));
        }

        public async Task<Statistic> UpdateStatisticAsync(string userId, Statistic statistic)
        {
            return await SendRequest(new HttpMethod("PATCH")
               , $"users/{userId}/statistics/{statistic.Name}.json"
               , statistic
               , async str => await Task.FromResult(statistic));
        }
    }
}
