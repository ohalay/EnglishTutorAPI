using EnglishTutor.Common.Interfaces;
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using System.Net.Http;
using EnglishTutor.Common.AppSettings;
using EnglishTutor.Services.JsonConverters;

namespace EnglishTutor.Services
{
    public class TranslateService : BaseService, ITranslateService
    {
        private readonly Translate _translateSettings;

        public TranslateService(IOptions<Translate> optionTranslate)
        {
            _translateSettings = optionTranslate.Value;
        }

        protected override Uri BaseUrl => _translateSettings.BaseUrl;

        public async Task<string> Translate(string from, string to, string text)
        {
            return await SendRequest<string>(HttpMethod.Get
                , $"translate?from={from}&to={to}&text={text}"
                , null
                , new JPathConverter("translationText")
                );
        }
    }
}
