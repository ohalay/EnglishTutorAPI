using EnglishTutor.Common.Interfaces;
using System.Threading.Tasks;
using System.Net.Http;
using EnglishTutor.Common.AppSettings;
using EnglishTutor.Services.JsonConverters;
using Microsoft.Extensions.Options;

namespace EnglishTutor.Services
{
    public class AuthService : BaseService<Auth>, IAuthService
    {
        public AuthService(IOptions<Auth> option) : base(option)
        {
        }

        public async Task<string> ValidateTokenAsync(string token)
        {
            return await SendRequest<string>(HttpMethod.Get
                , $"tokeninfo?access_token={token}"
                , deserializeConverter : new JPathConverter("sub"));
        }
    }
}
