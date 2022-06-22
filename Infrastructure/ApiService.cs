using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Infrastructure
{
    public interface IApiService
    {
        Task<T> GetAsync<T>(string endpoint);
        Task<T> PostAsync<T>(string endpoint, dynamic requestBody);
        Task<T> UpdateAsync<T>(string endpoint, dynamic requestBody);
        Task<T> DeleteAsync<T>(string endpoint, dynamic requestBody);
    }

    public class ApiService : IApiService
    {
        private readonly HttpClient _httpClient;

        public ApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<T> GetAsync<T>(string endpoint)
        {
            return await SendAsync<T>(endpoint, HttpMethod.Get);
        }
        public async Task<T> PostAsync<T>(string endpoint, dynamic requestBody)
        {
            return await SendAsync<T>(endpoint, HttpMethod.Post, requestBody);
        }
        public async Task<T> UpdateAsync<T>(string endpoint, dynamic requestBody)
        {
            return await SendAsync<T>(endpoint, HttpMethod.Put, requestBody);
        }

        public async Task<T> DeleteAsync<T>(string endpoint, dynamic requestBody)
        {
            return await SendAsync<T>(endpoint, HttpMethod.Delete, requestBody);
        }

        private async Task<T> SendAsync<T>(string endpoint, HttpMethod httpMethod, dynamic requestBody = null,
            CancellationToken cancellationToken = default)
        {
            _httpClient.DefaultRequestHeaders.Accept.Clear();
            var baseUrl = new Uri("").ToString();
            var url = $"{baseUrl.TrimEnd('/')}/{endpoint.TrimEnd('/')}";

            using var request = new HttpRequestMessage(httpMethod, url);

            //might move this to private method
            if (httpMethod == HttpMethod.Post || httpMethod == HttpMethod.Put)
            {
                var content = JsonConvert.SerializeObject(requestBody);
                var buffer = Encoding.UTF8.GetBytes(content);
                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                request.Content = byteContent;
            }

            var response = await _httpClient.SendAsync(request, cancellationToken);
            var responseContent = await response.Content.ReadAsStringAsync(cancellationToken);

            if (!response.IsSuccessStatusCode)
            {
                var errorMessage = $"Status Code:{response.StatusCode}; Response Message: {responseContent}";
                throw new Exception(errorMessage);
            }

            return JsonConvert.DeserializeObject<T>(responseContent);
        }
    }
}
