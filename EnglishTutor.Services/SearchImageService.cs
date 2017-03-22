using EnglishTutor.Common.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using System.Net.Http;
using EnglishTutor.Common.AppSettings;
using EnglishTutor.Services.JsonConverters;

namespace EnglishTutor.Services
{
    public class SearchImageService : BaseService<SearchImage>, ISearchImageService
    {
        public SearchImageService(IOptions<SearchImage> option) : base(option)
        {
        }

        public async Task<IEnumerable<string>> GetImages(string word, int count)
        {
            const string ORIENTATION = "horizontal";

            return await SendRequest<IEnumerable<string>>(HttpMethod.Get
                , $"?q={word}&key={ServiceSttings.ApiKey}&lang={LANG}&per_page={count}&orientation={ORIENTATION}"
                , deserializeConverter: new CollectionJPathConverter("hits[?(@)].previewURL")
                );
        }
    }
}
