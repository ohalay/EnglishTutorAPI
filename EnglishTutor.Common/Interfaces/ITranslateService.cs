using System.Collections.Generic;
using System.Threading.Tasks;
using EnglishTutor.Common.Dto;

namespace EnglishTutor.Common.Interfaces
{
    public interface ITranslateService
    {
        Task<string> Translate(string to, string text);

        Task<IEnumerable<Language>> GetSupportedLanguages();
    }
}