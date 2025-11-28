using ecommerce.Tests.Drivers;
using ecommerce.Tests.Models;
using ecommerce.Tests.Support;
using ecommerce_crud.DTO;
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

        [Given(@"que eu recebo um produto valido:")]
        public void DadoQueEuReceboUmProdutoValido(ProductCreateDto productDto)
        {
            _productRequest = productDto;
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
            _productResponse = (await _apiDriver.GetResponseBodyAs<ProductResponse>())!;

            if (_productResponse != null)
            {
                _scenarioContext["CreatedProductId"] = _productResponse.Id;
            }

            Assert.That(_productResponse, Is.Not.Null, "O produto é nulo.");
            Assert.That(_productResponse.Id, Is.GreaterThan(0), "Produto com ID inválido encontrado.");

            Assert.That(_productResponse.Model, Is.EqualTo(_productRequest.Model));
            Assert.That(_productResponse.Specifications, Is.EqualTo(_productRequest.Specifications));
            Assert.That(_productResponse.Price, Is.EqualTo(_productRequest.Price));
            Assert.That(_productResponse.StockQuantity, Is.EqualTo(_productRequest.StockQuantity));
            Assert.That(_productResponse.Type, Is.EqualTo((int)_productRequest.Type));
            Assert.That(_productResponse.ReleaseDate.Date, Is.EqualTo(_productRequest.ReleaseDate.Date));

            AssertHelpers.AssertTimestampsWereGenerated(
                _productResponse.CreatedAt,
                _productResponse.UpdatedAt,
                _productResponse.DeletedAt
            );
        }

        [Given(@"que eu tenho produtos validos:")]
        public async Task DadoQueEuTenhoProdutosValidos(ProductCreateDto productDto)
        {
            var response = await _apiDriver.PostAsync<ProductCreateDto>("api/products", productDto);

            Assert.That(response.IsSuccessStatusCode, Is.True, "Falha ao criar o produto de pré-condição.");

            var productResponse = await _apiDriver.GetResponseBodyAs<ProductResponse>();

            if (productResponse != null)
            {
                _scenarioContext["CreatedProductId"] = productResponse.Id;
            }
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
            Assert.That(_productListResponse!.Any(), Is.True, "A lista de produtos está vazia.");

            foreach (var product in _productListResponse)
            {
                Assert.That(product.Id, Is.GreaterThan(0));
                Assert.That(product.Model, Is.Not.Null.And.Not.Empty);

                AssertHelpers.AssertTimestampsAreValidForList(
                    product.CreatedAt,
                    product.UpdatedAt,
                    product.DeletedAt
                );
            }
        }

        [Given(@"que existe um produto com ID (.*) no sistema")]
        public void DadoQueExisteUmProdutoComIDNoSistema(int idIgnorado)
        {
            //TODO
        }

        [Then("o corpo da resposta deve conter os detalhes do produto com ID {int}")]
        public async Task ThenOCorpoDaRespostaDeveConterOsDetalhesDoProdutoComID(int idEsperadoNaUrl)
        {
            //TODO
        }
    }
}