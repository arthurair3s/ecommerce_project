using ecommerce.Tests.Drivers;
using ecommerce.Tests.Models;
using ecommerce.Tests.Support;
using ecommerce_crud.DTO;
using ecommerce_crud.Models.Enums;
using NUnit.Framework;
using Reqnroll;

namespace ecommerce.Tests.StepDefinitions
{
    [Binding]
    public class ProductsStepDefinitions
    {
        private readonly ApiDriver _apiDriver;
        private readonly ScenarioContext _scenarioContext;

        private ProductCreateDto _productRequest = null!;
        private HttpResponseMessage _response = null!;
        private ProductResponse _productResponse = null!;

        public ProductsStepDefinitions(ScenarioContext scenarioContext)
        {
            _scenarioContext = scenarioContext;
            _apiDriver = new ApiDriver();
        }

        [Given(@"que eu tenho um produto valido:")]
        public void DadoQueEuTenhoUmProdutoValido(Table table)
        {
            var row = table.Rows[0];

            // Converte o enum de string para ProductType
            var typeString = row["Type"];
            var productType = Enum.Parse<ProductType>(typeString);

            // Cria o DTO
            _productRequest = new ProductCreateDto
            {
                Model = row["Model"],
                ReleaseDate = DateTime.Parse(row["ReleaseDate"]),
                Specifications = row["Specifications"],
                Price = decimal.Parse(row["Price"]),
                StockQuantity = int.Parse(row["StockQuantity"]),
                Type = productType
            };
        }

        [When(@"eu envio uma requisição POST para ""(.*)""")]
        public async Task QuandoEuEnvioUmaRequisicaoPOSTPara(string endpoint)
        {
            _response = await _apiDriver.PostAsync<ProductCreateDto>(endpoint, _productRequest);
        }

        [Then(@"a resposta deve ter o código de status (.*)")]
        public void EntaoARespostaDeveTerOCodigoDeStatus(int statusCode)
        {
            Assert.That((int)_response.StatusCode, Is.EqualTo(statusCode));
        }

        [Then(@"o corpo da resposta deve conter os detalhes do produto criado")]
        public async Task EntaoOCorpoDaRespostaDeveConterOsDetalhes()
        {
            _productResponse = await _apiDriver.GetResponseBodyAs<ProductResponse>();

            Assert.That(_productResponse, Is.Not.Null);
            Assert.That(_productResponse.Id, Is.GreaterThan(0));

            // Valida dados
            Assert.That(_productResponse.Model, Is.EqualTo(_productRequest.Model));
            Assert.That(_productResponse.Specifications, Is.EqualTo(_productRequest.Specifications));
            Assert.That(_productResponse.Price, Is.EqualTo(_productRequest.Price));
            Assert.That(_productResponse.StockQuantity, Is.EqualTo(_productRequest.StockQuantity));
            Assert.That(_productResponse.Type, Is.EqualTo((int)_productRequest.Type));
            Assert.That(_productResponse.ReleaseDate.Date, Is.EqualTo(_productRequest.ReleaseDate.Date));

            // Valida timestamps
            AssertHelpers.AssertTimestampsWereGenerated(
                _productResponse.CreatedAt,
                _productResponse.UpdatedAt,
                _productResponse.DeletedAt
            );
        }
    }
}