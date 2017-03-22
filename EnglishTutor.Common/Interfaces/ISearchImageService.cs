using System.Collections.Generic;
using System.Threading.Tasks;

namespace EnglishTutor.Common.Interfaces
{
    public interface ISearchImageService
    {
        Task<IEnumerable<string>> GetImages(string word, int count);
    }
}