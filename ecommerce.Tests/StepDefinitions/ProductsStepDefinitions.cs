using ecommerce.Tests.Drivers;
using ecommerce.Tests.Models;
using ecommerce.Tests.Support;
using ecommerce_crud.DTO;
using NUnit.Framework;
using Reqnroll.Assist;
using System.Threading.Tasks;

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
            _productResponse = await _apiDriver.GetResponseBodyAs<ProductResponse>();

            _scenarioContext["CreatedProductId"] = _productResponse.Id;

            Assert.That(_productResponse, Is.Not.Null, "O produto é nulo.");
            Assert.That(_productResponse.Id, Is.GreaterThan(0), "Produto com ID inválido encontrado.");

            Assert.That(_productResponse.Model, Is.EqualTo(_productRequest.Model));
            Assert.That(_productResponse.ReleaseDate.Date, Is.EqualTo(_productRequest.ReleaseDate.Date));
            Assert.That(_productResponse.Specifications, Is.EqualTo(_productRequest.Specifications));
            Assert.That(_productResponse.Price, Is.EqualTo(_productRequest.Price));
            Assert.That(_productResponse.StockQuantity, Is.EqualTo(_productRequest.StockQuantity));
            Assert.That(_productResponse.Type, Is.EqualTo((int)_productRequest.Type));

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

            _productResponse = await _apiDriver.GetResponseBodyAs<ProductResponse>();

            Assert.That(_productResponse, Is.Not.Null, "A API retornou um corpo nulo na criação do produto.");

            _scenarioContext["CreatedProductId"] = _productResponse.Id;
        }

        [When(@"eu envio uma requisição GET para ""(.*)""")]
        public async Task QuandoEuEnvioUmaRequisicaoGETPara(string endpoint)
        {
            if (endpoint.Contains("<id>"))
            {
                var productId = (int)_scenarioContext["CreatedProductId"];
                endpoint = endpoint.Replace("<id>", productId.ToString());
            }

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

        [Given(@"que eu tenho um produto valido:")]
        public async Task DadoQueEuTenhoUmProdutoValido(ProductCreateDto productDto)
        {
            var response = await _apiDriver.PostAsync<ProductCreateDto>("api/products", productDto);

            Assert.That(response.IsSuccessStatusCode, Is.True, "Falha ao criar o produto de pré-condição.");

            _productResponse = await _apiDriver.GetResponseBodyAs<ProductResponse>();

            Assert.That(_productResponse, Is.Not.Null, "O produto retornado é nulo.");
            Assert.That(_productResponse.Id, Is.GreaterThan(0), "Produto retornou ID inválido.");

            _scenarioContext["CreatedProductId"] = _productResponse.Id;
            _scenarioContext["CreatedProductObj"] = _productResponse;
        }

        [Then(@"o corpo da resposta deve conter os detalhes do produto cadastrado")]
        public async Task EntaoOCorpoDaRespostaDeveConterOsDetalhesDoProdutoCadastrado()
        {
            var expected = (ProductResponse)_scenarioContext["CreatedProductObj"];

            _productResponse = await _apiDriver.GetResponseBodyAs<ProductResponse>();

            Assert.That(_productResponse.Model, Is.EqualTo(expected.Model));
            Assert.That(_productResponse.ReleaseDate.Date, Is.EqualTo(expected.ReleaseDate.Date));
            Assert.That(_productResponse.Specifications, Is.EqualTo(expected.Specifications));
            Assert.That(_productResponse.Price, Is.EqualTo(expected.Price));
            Assert.That(_productResponse.StockQuantity, Is.EqualTo(expected.StockQuantity));
            Assert.That(_productResponse.Type, Is.EqualTo(expected.Type));
        }

        [Given("eu recebo os novos dados do produto:")]
        public void DadoEuReceboOsNovosDadosDoProduto(Table table)
        {
            _productRequest = table.CreateInstance<ProductCreateDto>();
        }

        [When(@"eu envio uma requisição PUT para ""(.*)""")]
        public async Task QuandoEuEnvioUmaRequisicaoPUTPara(string endpoint)
        {
            if (endpoint.Contains("<id>"))
            {
                var productId = (int)_scenarioContext["CreatedProductId"];
                endpoint = endpoint.Replace("<id>", productId.ToString());
            }

            _response = await _apiDriver.PutAsync<ProductCreateDto>(endpoint, _productRequest);
        }

        [Then("o corpo da resposta deve conter os detalhes atualizados do produto")]
        public async Task EntaoOCorpoDaRespostaDeveConterOsDetalhesAtualizadosDoProduto()
        {
            _productResponse = await _apiDriver.GetResponseBodyAs<ProductResponse>();

            Assert.That(_productResponse, Is.Not.Null, "A resposta da atualização é nula.");

            Assert.That(_productResponse.Model, Is.EqualTo(_productRequest.Model));
            Assert.That(_productResponse.ReleaseDate.Date, Is.EqualTo(_productRequest.ReleaseDate.Date));
            Assert.That(_productResponse.Specifications, Is.EqualTo(_productRequest.Specifications));
            Assert.That(_productResponse.Price, Is.EqualTo(_productRequest.Price));
            Assert.That(_productResponse.StockQuantity, Is.EqualTo(_productRequest.StockQuantity));
            Assert.That(_productResponse.Type, Is.EqualTo((int)_productRequest.Type));

            var originalId = (int)_scenarioContext["CreatedProductId"];
            Assert.That(_productResponse.Id, Is.EqualTo(originalId), "O ID do produto mudou após o update, o que não deveria acontecer.");
        }
    }
}   