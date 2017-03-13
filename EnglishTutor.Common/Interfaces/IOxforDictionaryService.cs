using EnglishTutor.Common.Dto;
using System.Threading.Tasks;

namespace EnglishTutor.Common.Interfaces
{
    public interface IOxforDictionaryService
    {
        Task<string> GetNormalizedWordAsync(string name);

        Task<Word> GetWordAsync(string name);
    }
}
