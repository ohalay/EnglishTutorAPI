using System.Threading.Tasks;

namespace EnglishTutor.Common.Interfaces
{
    public interface ITranslateService
    {
        Task<string> Translate(string from, string to, string text);
    }
}