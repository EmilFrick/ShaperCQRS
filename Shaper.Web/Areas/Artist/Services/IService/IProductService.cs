using Shaper.Models.Entities;
using Shaper.Models.Models.ProductModels;

namespace Shaper.Web.Areas.Artist.Services.IService
{
    public interface IProductService
    {
        Task<ProductEntityModel> GetProductAsync(int id, string token);
        Task<IEnumerable<Product>> GetProductsAsync(string token);
        Task<ProductUpsertModel> GetProductVMsAsync(string token, int? id = null);
        Task<Product> GetProductWithComponentsAsync(ProductUpsertModel productVM, string token);
        Task<IEnumerable<Product>>  GetProductsByColorAsync(int colorId, string token);

        Task CreateProductAsync(Product product, string token);
        Task UpdateProductAsync(Product product, string token);
        Task DeleteProductAsync(int id, string token);
    }
}
