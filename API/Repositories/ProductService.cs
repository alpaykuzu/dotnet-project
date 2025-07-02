using API.Data;
using API.DTOs.Product;
using API.Interfaces;
using YirmibesYazilim.Framework.Models.Responses;

namespace API.Repositories
{
    public class ProductService : IProductService
    {
        private readonly AppDbContext _appDbContext;
        public ProductService(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
        public async Task<Response<ProductResponse>> AddProductAsync(ProductRequest product)
        {
            throw new NotImplementedException();
        }

        public Task<Response<NoContent>> DeleteProductAsync(int productId)
        {
            throw new NotImplementedException();
        }

        public Task<Response<ProductResponse>> GetProductAsync(int productId)
        {
            throw new NotImplementedException();
        }

        public Task<Response<ProductResponse>> UpdateProductAsync(ProductRequest product)
        {
            throw new NotImplementedException();
        }
    }
}
