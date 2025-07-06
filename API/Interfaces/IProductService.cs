using API.DTOs.Product;
using YirmibesYazilim.Framework.Models.Responses;

namespace API.Interfaces
{
    public interface IProductService
    {
        Task<Response<ProductResponse>> GetProductAsync(int productId);
        Task<Response<List<ProductResponse>>> GetProductAllAsync();
        Task<Response<NoContent>> AddProductAsync(ProductRequest product);
        Task<Response<NoContent>> UpdateProductAsync(int productId, ProductRequest newProduct);
        Task<Response<NoContent>> DeleteProductAsync(int productId);

    }
}
