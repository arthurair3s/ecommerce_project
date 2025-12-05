using ecommerce_crud.Data;
using ecommerce_crud.DTO;
using ecommerce_crud.Models;
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

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
            var products = await _appDbContext.Products.ToListAsync();
            return Ok(products);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            var product = await _appDbContext.Products.FindAsync(id);
            if (product == null)
                return NotFound();

            return Ok(product);
        }

        [HttpPost]
        public async Task<IActionResult> AddProduct(ProductCreateDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var now = DateTime.UtcNow;

            var product = new Product
            {
                Model = dto.Model,
                ReleaseDate = dto.ReleaseDate,
                Specifications = dto.Specifications,
                Price = dto.Price,
                StockQuantity = dto.StockQuantity,
                Type = dto.Type,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                DeletedAt = null
            };

            _appDbContext.Products.Add(product);
            await _appDbContext.SaveChangesAsync();

            return CreatedAtAction(nameof(GetProduct), new { id = product.Id }, product);
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> PatchProduct(int id, [FromBody] JsonPatchDocument<ProductPatchDto> patchDoc)
        {
            if (patchDoc == null)
                return BadRequest();

            var product = await _appDbContext.Products.FindAsync(id);
            if (product == null)
                return NotFound();

            var dto = new ProductPatchDto
            {
                Model = product.Model,
                ReleaseDate = product.ReleaseDate,
                Specifications = product.Specifications,
                Price = product.Price,
                StockQuantity = product.StockQuantity,
                Type = product.Type
            };

            patchDoc.ApplyTo(dto, ModelState);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (dto.Model != null) product.Model = dto.Model;
            if (dto.ReleaseDate.HasValue) product.ReleaseDate = dto.ReleaseDate.Value;
            if (dto.Specifications != null) product.Specifications = dto.Specifications;
            if (dto.Price.HasValue) product.Price = dto.Price.Value;
            if (dto.StockQuantity.HasValue) product.StockQuantity = dto.StockQuantity.Value;
            if (dto.Type.HasValue) product.Type = dto.Type.Value;

            product.UpdatedAt = DateTime.UtcNow;

            await _appDbContext.SaveChangesAsync();

            return NoContent();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(int id, [FromBody] ProductUpdateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var product = await _appDbContext.Products.FindAsync(id);
            if (product == null)
                return NotFound();

            product.Model = dto.Model;
            product.ReleaseDate = dto.ReleaseDate;
            product.Specifications = dto.Specifications;
            product.Price = dto.Price;
            product.StockQuantity = dto.StockQuantity;
            product.Type = dto.Type;
            product.UpdatedAt = DateTime.UtcNow;

            await _appDbContext.SaveChangesAsync();

            return Ok(product);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var product = await _appDbContext.Products.FindAsync(id);
            if (product == null)
                return NotFound();

            _appDbContext.Products.Remove(product);

            await _appDbContext.SaveChangesAsync();

            return NoContent();
        }
    }
}
