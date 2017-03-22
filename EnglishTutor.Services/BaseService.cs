using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using EnglishTutor.Common.AppSettings;
using EnglishTutor.Common.Exception;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Serialization;

namespace EnglishTutor.Services
{
    public  class BaseService<TOption> where TOption : ServiceSetting, new()
    {
        protected const string LANG = "en";


        private static HttpClient _httpClient;
        private readonly TOption _serviceSetting;

        public BaseService(IOptions<TOption> optionService)
        {
            _serviceSetting = optionService.Value;
        }

        protected TOption ServiceSttings => _serviceSetting;

        private static HttpClient HttpClient
        {
            get
            {
                return _httpClient ?? (_httpClient = new HttpClient());
            }
        }


        protected virtual IEnumerable<Tuple<string, string>> GetCustomHeaders()
        {
            return new List<Tuple<string, string>>();
        }

        protected async Task<TResult> SendRequest<TResult>(HttpMethod method, string url, object body = null,  JsonConverter deserializeConverter = null)
        {
            try
            {
                var requestMessage = new HttpRequestMessage(method, $"{ServiceSttings.BaseUrl.AbsoluteUri}{url}");

                if (body != null)
                    requestMessage.Content = new StringContent(JsonConvert.SerializeObject(body, new JsonSerializerSettings()
                    {
                        ContractResolver = new CamelCasePropertyNamesContractResolver()
                    } ));

                foreach (var item in GetCustomHeaders())
                {
                    requestMessage.Headers.Add(item.Item1, item.Item2);
                }

                var response = await HttpClient.SendAsync(requestMessage);

                response.EnsureSuccessStatusCode();

                var stringResponce = await response.Content.ReadAsStringAsync();

                return deserializeConverter == null ? 
                    JsonConvert.DeserializeObject<TResult>(stringResponce) 
                    : JsonConvert.DeserializeObject<TResult>(stringResponce, deserializeConverter);
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
