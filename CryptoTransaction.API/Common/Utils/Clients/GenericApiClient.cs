using Newtonsoft.Json;
using CryptoTransaction.API.Common.Utils.Interface;

namespace CryptoTransaction.API.Common.Utils.Clients
{
    public class GenericApiClient: IGenericApiClient
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public GenericApiClient(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<T> GetAsync<T>(string url)
        {
            using (var client = _httpClientFactory.CreateClient())
            {
                var response = await client.GetAsync(url);
                response.EnsureSuccessStatusCode();

                var content = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<T>(content);
            }
        }
    }
}
