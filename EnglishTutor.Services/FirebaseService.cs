using EnglishTutor.Common.Interfaces;
using EnglishTutor.Common.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Net.Http;
using EnglishTutor.Common.AppSettings;
using EnglishTutor.Services.JsonConverters;
using Microsoft.Extensions.Options;

namespace EnglishTutor.Services
{
    public class FirebaseService : BaseService<Firebase>, IFirebaseService
    {

        public FirebaseService(IOptions<Firebase> option) : base(option)
        {
        }

        private async Task<Word> GetWordAsync(string name)
        {
            return await SendRequest<Word>(HttpMethod.Get, $"vocabulary/{name}.json");
        }

        public async Task<IEnumerable<Statistic>> GetStatisticsAsync(string userId, int? limitTo)
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
            var name = word.Name;
            word.Name = null;

            return await SendRequest<Word>(new HttpMethod("PATCH")
               , $"vocabulary/{name}.json"
               , word);
        }

        public async Task<Statistic> UpdateWordStatisticAsync(string userId, Statistic statistic)
        {
            var name = statistic.Name;
            statistic.Name = null;
            return await SendRequest<Statistic>(new HttpMethod("PATCH")
               , $"users/{userId}/statistics/{name}.json"
               , statistic);
        }

        public async Task<User> GetUser(string userId)
        {
            return await SendRequest<User>(HttpMethod.Get
               , $"users/{userId}.json");
        }

        public async Task<User> CreateUser(string userId, User user)
        {
            return await SendRequest<User>(new HttpMethod("PATCH")
               , $"users/{userId}.json"
               , user);
        }

        public async Task<Statistic> GetWordStatisticAsync(string userId, string name)
        {
            return await SendRequest<Statistic>(HttpMethod.Get
                , $"users/{userId}/statistics/{name}.json");
        }

        public async Task<string> UpdateWordTranslation(string name, string leng, string translation)
        {
            return await SendRequest<string>(new HttpMethod("PATCH")
               , $"vocabulary/{name}/translations/{leng}.json"
               , translation);
        }
    }
}
