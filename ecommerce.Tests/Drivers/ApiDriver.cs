using Microsoft.Extensions.Configuration;
using System.Net.Http.Json;
using System.Text.Json;

namespace ecommerce.Tests.Drivers
{
    public class ApiDriver
    {
        private readonly HttpClient _httpClient;
        private HttpResponseMessage? _lastResponse;

        public ApiDriver(string? baseUrl = null)
        {

            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            var configUrl = configuration["ApiSettings:BaseUrl"];
            var apiBaseUrl = baseUrl ?? configUrl ?? "https://localhost:7113";

            var handler = new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true
            };


            _httpClient = new HttpClient(handler)
            {
                BaseAddress = new Uri(apiBaseUrl)
            };
        }

        public async Task<HttpResponseMessage> PostAsync<TRequest>(string endpoint, TRequest requestBody)
        {
            _lastResponse = await _httpClient.PostAsJsonAsync(endpoint, requestBody);
            return _lastResponse;
        }

        public async Task<HttpResponseMessage> GetAsync(string endpoint)
        {
            _lastResponse = await _httpClient.GetAsync(endpoint);
            return _lastResponse;
        }

        public HttpResponseMessage GetLastResponse()
        {
            if (_lastResponse == null)
                throw new InvalidOperationException("Nenhuma requisição foi feita ainda.");

            return _lastResponse;
        }

        public async Task<TResponse?> GetResponseBodyAs<TResponse>()
        {
            if (_lastResponse == null)
                throw new InvalidOperationException("Nenhuma requisição foi feita ainda.");

            var content = await _lastResponse.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<TResponse>(content, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
        }

        public async Task<string> GetResponseBodyAsString()
        {
            if (_lastResponse == null)
                throw new InvalidOperationException("Nenhuma requisição foi feita ainda.");

            return await _lastResponse.Content.ReadAsStringAsync();
        }

        public async Task<HttpResponseMessage> DeleteAsync(string endpoint)
        {
            _lastResponse = await _httpClient.DeleteAsync(endpoint);
            return _lastResponse;
        }

        public async Task<HttpResponseMessage> PutAsync <TRequest>(string endpoint, TRequest requestBody)
        {
            _lastResponse = await _httpClient.PutAsJsonAsync(endpoint, requestBody);
            return _lastResponse;
        }

        public async Task<HttpResponseMessage> PatchAsync(string endpoint, string jsonPatchPayload)
        {
            var content = new StringContent(jsonPatchPayload, System.Text.Encoding.UTF8, "application/json-patch+json");

            var request = new HttpRequestMessage(new HttpMethod("PATCH"), endpoint)
            {
                Content = content
            };

            _lastResponse = await _httpClient.SendAsync(request);
            return _lastResponse;
        }
    }
}