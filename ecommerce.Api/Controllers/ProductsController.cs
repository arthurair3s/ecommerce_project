using ecommerce_crud.DTO;
using ecommerce_crud.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ecommerce_crud.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly AppDbContext _appDbContext;

        public ProductsController(AppDbContext appDbContext)
        { 
            _appDbContext = appDbContext;
        }

        /// <summary>
        /// Retorna todos os produtos cadastrados
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
            var products = await _appDbContext.Products.ToListAsync();

            return Ok(products);
        }

        /// <summary>
        /// Retorna um produto pelo ID específico
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            var product = await _appDbContext.Products.FindAsync(id);

            if (product == null)
            {
                return NotFound();
            }

            return product;
        }

        /// <summary>
        /// Cria um novo produto
        /// </summary>
        /// <param name="product"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> AddProduct(Product product)
        {
            _appDbContext.Products.Add(product);
            await _appDbContext.SaveChangesAsync();

            return CreatedAtAction(nameof(GetProduct), new { id = product.Id }, product);
        }

        /// <summary>
        /// Atualiza parcialmente um produto já existente
        /// </summary>
        /// <param name="id"></param>
        /// <param name="patchDoc"></param>
        /// <returns></returns>
        [HttpPatch("{id}")]
        public async Task<IActionResult> PatchProduct(int id, [FromBody] JsonPatchDocument<ProductPatchDto> patchDoc)
        {
            var product = await _appDbContext.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            var dto = new ProductPatchDto
            {
                Model = product.Model,
                Specifications = product.Specifications,
                Price = product.Price,
                StockQuantity = product.StockQuantity,
                Type = product.Type
            };

            patchDoc.ApplyTo(dto, ModelState);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (dto.Model != null) product.Model = dto.Model;
            if (dto.Specifications != null) product.Specifications = dto.Specifications;
            if (dto.Price.HasValue) product.Price = dto.Price.Value;
            if (dto.StockQuantity.HasValue) product.StockQuantity = dto.StockQuantity.Value;
            if (dto.Type.HasValue) product.Type = dto.Type.Value;


            await _appDbContext.SaveChangesAsync();

            return NoContent();
        }

        /// <summary>
        /// Atualiza completamente um projuto já existente
        /// </summary>
        /// <param name="id"></param>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(int id, [FromBody] ProductUpdateDto dto)
        {

            var product = await _appDbContext.Products.FindAsync(id);

            if (product == null)
            {
                return NotFound();
            }

            product.Model = dto.Model;
            product.Specifications = dto.Specifications;
            product.Price = dto.Price;
            product.StockQuantity = dto.StockQuantity;
            product.Type = dto.Type;

            await _appDbContext.SaveChangesAsync();

            return NoContent();
        }

        /// <summary>
        /// Deleta um produto pelo ID específico
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            if (id == null)
            {
                return BadRequest();
            }

            var product = await _appDbContext.Products.FindAsync(id);

            if (product == null)
            {
                return NotFound();
            }

            _appDbContext.Products.Remove(product);
            
            await _appDbContext.SaveChangesAsync();

            return NoContent();
        }

    }
}
