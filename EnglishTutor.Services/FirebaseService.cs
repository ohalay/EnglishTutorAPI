using EnglishTutor.Common.Interfaces;
using System;
using EnglishTutor.Common.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Net.Http;
using EnglishTutor.Common.AppSettings;
using EnglishTutor.Services.JsonConverters;
using Microsoft.Extensions.Options;

namespace EnglishTutor.Services
{
    public class FirebaseService : BaseService, IFirebaseService
    {
        private readonly Firebase _firebaseSettins;

        public FirebaseService(IOptions<Firebase> optionFirebase)
        {
            _firebaseSettins = optionFirebase.Value;
        }

        protected override Uri BaseUrl => _firebaseSettins.BaseUrl;

        private async Task<Word> GetWordAsync(string name)
        {
            return await SendRequest<Word>(HttpMethod.Get, $"vocabulary/{name}.json");
        }

        public async Task<IEnumerable<Statistic>> GetStatisticAsync(string userId, int? limitTo)
        {
            return await SendRequest<IEnumerable<Statistic>>(HttpMethod.Get
                , $"users/{userId}/statistics.json?orderBy=\"timestamp\"&limitToLast={limitTo}"
                , null
                , new StatisticConverter());

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
            return await SendRequest<Word>(new HttpMethod("PATCH")
               , $"vocabulary/{word.Name}.json"
               , word);
        }

        public async Task<Statistic> UpdateStatisticAsync(string userId, Statistic statistic)
        {
            return await SendRequest<Statistic>(new HttpMethod("PATCH")
               , $"users/{userId}/statistics/{statistic.Name}.json"
               , statistic);
        }
    }
}
