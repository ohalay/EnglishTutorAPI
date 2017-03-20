using System.Threading.Tasks;

namespace EnglishTutor.Common.Interfaces
{
    public interface IAccountService
    {
        Task<string> ValidateTokenAsync(string token);
    }
}
