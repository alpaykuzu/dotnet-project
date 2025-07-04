using API.DTOs.Product;
using API.Interfaces;
using API.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;
        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet("get-product")]
        public async Task<IActionResult> GetProduct(int productId) 
        {
            return Ok(await _productService.GetProductAsync(productId));
        }
        [HttpPost("add-product")]
        public async Task<IActionResult> AddProduct(ProductRequest product)
        {
            return Ok(await _productService.AddProductAsync(product));
        }
        [HttpPut("update-product")]
        public async Task<IActionResult> UpdateProduct(int productId, ProductRequest productRequest)
        {
            return Ok(await _productService.UpdateProductAsync(productId, productRequest));
        }
        [HttpDelete("delete-product")]
        public async Task<IActionResult> DeleteProduct(int productId)
        {
            return Ok(await _productService.DeleteProductAsync(productId));
        }
    }
}
