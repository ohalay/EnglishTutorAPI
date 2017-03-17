using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using EnglishTutor.Common.Exception;

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

        protected virtual IEnumerable<Tuple<string, string>> GetCustomHeaders()
        {
            return new List<Tuple<string, string>>();
        }

        protected async Task<T> SendRequest<T>(HttpMethod method, string url, object body = null,  JsonConverter deserializeConverter = null)
        {
            try
            {
                var requestMessage = new HttpRequestMessage(method, $"{BaseUrl.AbsoluteUri}{url}")
                {
                    Content = new StringContent(JsonConvert.SerializeObject(body))
                };

                foreach(var item in GetCustomHeaders())
                {
                    requestMessage.Headers.Add(item.Item1, item.Item2);
                }

                var response = await HttpClient.SendAsync(requestMessage);

                response.EnsureSuccessStatusCode();

                var stringResponce = await response.Content.ReadAsStringAsync();

                return deserializeConverter == null ? 
                    JsonConvert.DeserializeObject<T>(stringResponce) 
                    : JsonConvert.DeserializeObject<T>(stringResponce, deserializeConverter);
            }
            catch(HttpRequestException e)
            {
                throw new ApiException(ApiError.AccessForbidden)
                {
                    ErrorData = e.Message
                };
            }
        }
    }
}
