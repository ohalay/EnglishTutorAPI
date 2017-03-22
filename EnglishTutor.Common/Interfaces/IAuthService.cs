using System.Threading.Tasks;

namespace EnglishTutor.Common.Interfaces
{
    public interface IAuthService
    {
        Task<string> ValidateTokenAsync(string token);
    }
}
