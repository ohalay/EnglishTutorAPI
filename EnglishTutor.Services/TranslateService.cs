using EnglishTutor.Common.Interfaces;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using System.Net.Http;
using EnglishTutor.Common.AppSettings;
using EnglishTutor.Services.JsonConverters;

namespace EnglishTutor.Services
{
    public class TranslateService : BaseService<Translate>, ITranslateService
    {

        public TranslateService(IOptions<Translate> option) : base (option)
        {
        }

        public async Task<string> Translate(string to, string text)
        {
            return await SendRequest<string>(HttpMethod.Get
                , $"translate?from={LANG}&to={to}&text={text}"
                , null
                , new JPathConverter("translationText")
                );
        }
    }
}
