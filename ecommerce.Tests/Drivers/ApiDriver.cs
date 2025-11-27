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
            var apiBaseUrl = baseUrl ?? "https://localhost:7113";

            var handler = new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true
            };


            _httpClient = new HttpClient(handler)
            {
                BaseAddress = new Uri("https://localhost:7113")
            };
        }

        public async Task<HttpResponseMessage> PostAsync<TRequest>(string endpoint, TRequest requestBody)
        {
            _lastResponse = await _httpClient.PostAsJsonAsync(endpoint, requestBody);
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
    }
}