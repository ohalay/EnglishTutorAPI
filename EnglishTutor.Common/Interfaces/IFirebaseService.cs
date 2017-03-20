﻿using EnglishTutor.Common.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EnglishTutor.Common.Interfaces
{
    public interface IFirebaseService
    {
        Task<IEnumerable<Word>> GetWordsAsync(params string[] wordNames);

        Task<IEnumerable<Statistic>> GetStatisticsAsync(string userId, int? limitTo);

        Task<Statistic> GetWordStatisticAsync(string userId, string name);

        Task<Word> UpdateWordAsync(Word word);

        Task<Statistic> UpdateWordStatisticAsync(string userId, Statistic statistic);

        Task<User> GetUser(string userId);

        Task<User> CreateUser(string userId, User user);

    }
}
