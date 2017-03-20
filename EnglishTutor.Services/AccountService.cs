using EnglishTutor.Common.Interfaces;
using System;
using System.Threading.Tasks;
using System.Net.Http;
using EnglishTutor.Common.AppSettings;
using EnglishTutor.Services.JsonConverters;
using Microsoft.Extensions.Options;

namespace EnglishTutor.Services
{
    public class AccountService : BaseService, IAccountService
    {
        private readonly Account _optionSettings;

        public AccountService(IOptions<Account> optionAccount)
        {
            _optionSettings = optionAccount.Value;
        }
        protected override Uri BaseUrl => _optionSettings.BaseUrl;

        public async Task<string> ValidateTokenAsync(string token)
        {
            return await SendRequest<string>(HttpMethod.Get
                , $"tokeninfo?access_token={token}"
                , deserializeConverter : new JPathConverter("sub"));
        }
    }
}
