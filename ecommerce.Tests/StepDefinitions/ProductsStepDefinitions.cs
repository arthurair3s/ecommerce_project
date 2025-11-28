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
        private List<ProductResponse>? _productListResponse;

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
                ReleaseDate = DateTime.SpecifyKind(
                    DateTime.Parse(row["ReleaseDate"]),
                    DateTimeKind.Utc
                ),
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

            Assert.That(_productResponse, Is.Not.Null, "O produto é nulo.");
            Assert.That(_productResponse.Id, Is.GreaterThan(0), "Produto com ID inválido encontrado.");

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

        [Given(@"que existem produtos cadastrados no sistema")]
        public void DadoQueExistemProdutosCadastradosNoSistema()
        {
        }

        [When(@"eu envio uma requisição GET para ""(.*)""")]
        public async Task QuandoEuEnvioUmaRequisicaoGETPara(string endpoint)
        {
            _response = await _apiDriver.GetAsync(endpoint);
        }

        [Then(@"o corpo da resposta deve conter a lista de produtos cadastrados")]
        public async Task EntaoOCorpoDaRespostaDeveConterAListaDeProdutosCadastrados()
        {
            _productListResponse = await _apiDriver.GetResponseBodyAs<List<ProductResponse>>();

            Assert.That(_productListResponse, Is.Not.Null, "A lista de produtos é nula.");
            Assert.That(_productListResponse.Any(), Is.True, "A lista de produtos está vazia.");

            foreach (var product in _productListResponse)
            {
                // Valida ID
                Assert.That(product.Id, Is.GreaterThan(0), "Produto com ID inválido encontrado.");

                // Valida propriedades básicas
                Assert.That(product.Model, Is.Not.Null.And.Not.Empty);
                Assert.That(product.Specifications, Is.Not.Null.And.Not.Empty);
                Assert.That(product.Price, Is.GreaterThan(0));
                Assert.That(product.StockQuantity, Is.GreaterThanOrEqualTo(0));
                Assert.That(product.Type, Is.GreaterThanOrEqualTo(0));

                AssertHelpers.AssertTimestampsAreValidForList(
                    product.CreatedAt,
                    product.UpdatedAt,
                    product.DeletedAt
                );

            }

        }

        [Given(@"que existe um produto com ID (.*) no sistema")]
        public void DadoQueExisteUmProdutoComIDNoSistema(int Id)
        {
            //TODO
        }

        [Then("o corpo da resposta deve conter os detalhes do produto com ID {int}")]
        public void ThenOCorpoDaRespostaDeveConterOsDetalhesDoProdutoComID(int p0)
        {
            //TODO
        }


    }
}