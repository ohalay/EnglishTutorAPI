using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace EnglishTutor.Services
{
    public abstract class BaseService
    {
        private static HttpClient _httpClient;
        private static HttpClient HttpClient {
            get
            {
                return _httpClient ?? (_httpClient = new HttpClient());
            }
        }
        protected abstract Uri BaseUrl
        {
            get;
        }

        protected async Task<T> SendRequest<T>(HttpMethod method, string url, Func<string, Task<T>> deserialize = null)
        {
            try
            {
                var requestMessage = new HttpRequestMessage(method, $"{BaseUrl.AbsoluteUri}{url}");

                var response = await HttpClient.SendAsync(requestMessage);

                response.EnsureSuccessStatusCode();

                var stringResponce = await response.Content.ReadAsStringAsync();

                if (deserialize == null)
                    return JsonConvert.DeserializeObject<T>(stringResponce);
                else
                    return await deserialize(stringResponce);
            }
            catch(HttpRequestException e)
            {
                throw;
            }
        }
    }
}
