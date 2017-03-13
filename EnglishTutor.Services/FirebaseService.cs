using EnglishTutor.Common.Interfaces;
using System;
using EnglishTutor.Common.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace EnglishTutor.Services
{
    public class FirebaseService : BaseService, IFirebaseService
    {
        private readonly Uri _baseUrl = new Uri("https://eanglish-tutor.firebaseio.com");
        protected override Uri BaseUrl
        {
            get { return _baseUrl; }
        }

        public async Task<IEnumerable<Statistic>> GetStatisticAsync(string userId, int limitTo)
        {
            return await SendRequest(HttpMethod.Get
                , $"users/{userId}/statistics.json?orderBy=\"timestamp\"&limitToLast={limitTo}"
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

        private async Task<Word> GetWordAsync(string name)
        {
            return await SendRequest<Word>(HttpMethod.Get, $"vocabulary/{name}.json");
        }


    }
}
