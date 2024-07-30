using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace company.Controllers
{
    [Route("company/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly ProductService _productService;

        public ProductController(ProductService productService)
        {
            _productService = productService;
        }

        [HttpGet("list")]
        public async Task<ActionResult<List<Product>>> GetAllProducts()
        {
            var products = await _productService.GetAllProductsAsync();
            return Ok(products);
        }

        [HttpGet("detail/{id}")]
        public async Task<ActionResult<Product>> GetProductById(int id)
        {
            var product = await _productService.GetProductByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            return Ok(product);
        }

        [HttpPost("insert")]
        public async Task<ActionResult<Product>> InsertProduct(Product product)
        {
            var insertedProduct = await _productService.InsertProductAsync(product);
            return CreatedAtAction(nameof(GetProductById), new { id = insertedProduct.Pno }, insertedProduct);
        }

        [HttpPost("update")]
        public async Task<IActionResult> UpdateProduct(Product product)
        {
            var success = await _productService.UpdateProductAsync(product);
            if (!success)
            {
                return NotFound();
            }
            return NoContent();
        }

        [HttpPost("delete/{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var success = await _productService.DeleteProductAsync(id);
            if (!success)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}