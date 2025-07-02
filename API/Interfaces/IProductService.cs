using API.DTOs.Product;
using YirmibesYazilim.Framework.Models.Responses;

namespace API.Interfaces
{
    public interface IProductService
    {
        Task<Response<ProductResponse>> GetProductAsync(int productId);
        Task<Response<ProductResponse>> AddProductAsync(ProductRequest product);
        Task<Response<ProductResponse>> UpdateProductAsync(ProductRequest product);
        Task<Response<NoContent>> DeleteProductAsync(int productId);

    }
}
