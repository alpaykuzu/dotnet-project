using API.Data;
using API.DTOs.Product;
using API.Entities;
using API.Interfaces;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Net;
using YirmibesYazilim.Framework.Models.Responses;

namespace API.Repositories
{
    public class ProductService : IProductService
    {
        private readonly AppDbContext _appDbContext;
        private readonly IMapper _mapper;
        public ProductService(AppDbContext appDbContext, IMapper mapper)
        {
            _appDbContext = appDbContext;
            _mapper = mapper;
        }
        public async Task<Response<NoContent>> AddProductAsync(ProductRequest product)
        {
            var newProduct = _mapper.Map<ProductRequest,Product>(product);
            await _appDbContext.Products.AddAsync(newProduct);
            await _appDbContext.SaveChangesAsync();
            return Response<NoContent>.Success(HttpStatusCode.OK, "Ürün Ekleme Başarılı!");
        }

        public async Task<Response<NoContent>> DeleteProductAsync(int productId)
        {
            var product = await _appDbContext.Products.FirstOrDefaultAsync(x => x.Id == productId);
            if (product == null)
            {
                return Response<NoContent>.Fail("Silme Başarısız", HttpStatusCode.BadRequest);
            }
            else
            {
                _appDbContext.Products.Remove(product);
                _appDbContext.SaveChanges();
                return Response<NoContent>.Success(HttpStatusCode.OK, "Silme Başarılı!");
            }
        }

        public async Task<Response<ProductResponse>> GetProductAsync(int productId)
        {
            var product = await _appDbContext.Products.FirstOrDefaultAsync(x => x.Id == productId);
            if(product == null)
            {
                return Response<ProductResponse>.Fail("Sorgu Başarısız", HttpStatusCode.BadRequest);
            }
            else
            {
                var response = _mapper.Map<Product, ProductResponse>(product);
                return Response<ProductResponse>.Success(response, HttpStatusCode.OK ,"Başarılı!");
            }
        }

        public async Task<Response<NoContent>> UpdateProductAsync(int productId, ProductRequest newProduct)
        {
            var product = await _appDbContext.Products.FirstOrDefaultAsync(x => x.Id == productId);
            if(product == null)
            {
                return Response<NoContent>.Fail("Güncelleme Başarısız", HttpStatusCode.BadRequest);
            }
            else
            {
                product.ProductName = newProduct.ProductName;
                product.ProductDescription = newProduct.ProductDescription;
                product.ProductPrice = newProduct.ProductPrice;
                product.ProductQuantity = newProduct.ProductQuantity;

                _appDbContext.Products.Update(product);
                _appDbContext.SaveChanges();
                return Response<NoContent>.Success(HttpStatusCode.OK, "Güncelleme Başarılı!");
            }
        }
    }
}
