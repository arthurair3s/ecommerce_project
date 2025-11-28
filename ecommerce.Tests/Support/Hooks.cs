using ecommerce.Tests.Drivers;
using Reqnroll;

namespace ecommerce.Tests.Support
{
    [Binding]
    public class Hooks
    {
        private readonly ScenarioContext _scenarioContext;
        private readonly ApiDriver _apiDriver;

        public Hooks(ScenarioContext scenarioContext)
        {
            _scenarioContext = scenarioContext;
            _apiDriver = new ApiDriver();
        }

        [AfterScenario]
        public async Task CleanUpData()
        {
            if (_scenarioContext.ContainsKey("CreatedProductId"))
            {
                var id = (int)_scenarioContext["CreatedProductId"];

                Console.WriteLine($"Faxina via API: Deletando produto ID {id}...");

                var response = await _apiDriver.DeleteAsync($"/api/products/{id}");

                if (!response.IsSuccessStatusCode)
                {
                    Console.WriteLine($"Erro ao limpar dados: {response.StatusCode}");
                }
            }
        }
    }
}