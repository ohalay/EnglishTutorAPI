using EnglishTutor.Common.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EnglishTutor.Common.Interfaces
{
    public interface IFirebaseService
    {
        Task<IEnumerable<Word>> GetWordsAsync(params string[] wordNames);

        Task<IEnumerable<Statistic>> GetStatisticAsync(string userId, int limitTo);
    }
}
