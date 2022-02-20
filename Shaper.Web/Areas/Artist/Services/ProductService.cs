using Newtonsoft.Json;
using Shaper.DataAccess.IdentityContext;
using Shaper.Models.Entities;
using Shaper.Models.Models.ProductComponentsModels;
using Shaper.Models.Models.ProductModels;
using Shaper.Models.Models.ShoppingCartModels;
using Shaper.Utility;
using Shaper.Web.ApiService.IService;
using Shaper.Web.Areas.Artist.Services.IService;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Security.Policy;
using System;

namespace Shaper.Web.Areas.Admin.Services
{
    public class ProductService : IProductService
    {
        private readonly IShaperApiService _apiService;
        private readonly IHttpClientFactory _httpClient;


        public ProductService(IShaperApiService apiService, IdentityAppDbContext db, IHttpClientFactory httpClient)
        {
            _apiService = apiService;
            _httpClient = httpClient;
        }

        public async Task<ProductEntityModel> GetProductAsync(int id, string token)
        {
            var client = _httpClient.CreateClient();
            var req = new HttpRequestMessage(HttpMethod.Get, ApiPaths.ApiPath.Products.GetEndpoint(id));
            if (token != null && token.Length != 0)
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
            var res = await client.SendAsync(req);
            if (res.IsSuccessStatusCode)
            {
                var jsonString = await res.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<ProductEntityModel>(jsonString);
            }
            else
            {
                return null;
            }
        }

        public async Task<IEnumerable<Product>> GetProductsAsync(string token)
        {
            var client = _httpClient.CreateClient();
            if (token != null && token.Length != 0)
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
            var req = new HttpRequestMessage(HttpMethod.Get, ApiPaths.ApiPath.Products.GetEndpoint(null));
            var res = await client.SendAsync(req);

            if (res.IsSuccessStatusCode)
            {
                var jsonString = await res.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<IEnumerable<Product>>(jsonString);
            }

            return null;
        }

        public async Task CreateProductAsync(Product product, string token)
        {
            if (product != null)
            {
                var client = _httpClient.CreateClient();
                var req = new HttpRequestMessage(HttpMethod.Post, ApiPaths.ApiPath.Products.GetEndpoint(null));
                req.Content = new StringContent(JsonConvert.SerializeObject(product), Encoding.UTF8, "application/json");
                if (token != null && token.Length != 0)
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                }
                var res = await client.SendAsync(req);
            }
        }

        public async Task UpdateProductAsync(Product product, string token)
        {
            if (product != null)
            {
                var client = _httpClient.CreateClient();
                var req = new HttpRequestMessage(HttpMethod.Put, ApiPaths.ApiPath.Products.GetEndpoint(product.Id));
                req.Content = new StringContent(JsonConvert.SerializeObject(product), Encoding.UTF8, "application/json");
                if (token != null && token.Length != 0)
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                }
                var res = await client.SendAsync(req);
            }
        }

        public async Task DeleteProductAsync(int id, string token)
        {
            var client = _httpClient.CreateClient();
            if (token != null && token.Length != 0)
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
            var req = new HttpRequestMessage(HttpMethod.Delete, ApiPaths.ApiPath.Products.GetEndpoint(id));
            var res = await client.SendAsync(req);
        }

        public async Task<ProductUpsertModel> GetProductVMsAsync(string token, int? id = null)
        {
            string url = "";
            if (id is null || id == 0)
                url = ApiPaths.ApiPath.ProductsVM.GetEndpoint();
            else
                url = ApiPaths.ApiPath.ProductsVM.GetEndpoint(id);

            var client = _httpClient.CreateClient();
            if (token != null && token.Length != 0)
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
            var req = new HttpRequestMessage(HttpMethod.Get, url);
            var res = await client.SendAsync(req);

            if (res.IsSuccessStatusCode)
            {
                var jsonString = await res.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<ProductUpsertModel>(jsonString);
            }
            return null;
        }

        public async Task<Product> GetProductWithComponentsAsync(ProductUpsertModel upsertVM, string token)
        {
            ProductReqComponentsModel requestingComponents = new(upsertVM.ColorId, upsertVM.ShapeId, upsertVM.TransparencyId);
            ProductResComponentsModel components = new();

            var client = _httpClient.CreateClient();
            if (token != null && token.Length != 0)
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
            var req = new HttpRequestMessage(HttpMethod.Get, ApiPaths.ApiPath.ProductComponents.GetEndpoint());
            req.Content = new StringContent(JsonConvert.SerializeObject(requestingComponents), Encoding.UTF8, "application/json");
            var res = await client.SendAsync(req);

            if (res.IsSuccessStatusCode)
            {
                var jsonString = await res.Content.ReadAsStringAsync();
                components = JsonConvert.DeserializeObject<ProductResComponentsModel>(jsonString);
            }
            else
            {
                return null;
            }

            var product = upsertVM.VmToNewProduct(components);
            return product;

        }

        public async Task<IEnumerable<Product>> GetProductsByColorAsync(int colorId, string token)
        {
            var client = _httpClient.CreateClient();
            if (token != null && token.Length != 0)
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            var req = new HttpRequestMessage(HttpMethod.Get, ApiPaths.ApiPath.ProductsByColor.GetEndpoint(colorId));
            var res = await client.SendAsync(req);

            if (res.IsSuccessStatusCode)
            {
                var jsonString = await res.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<IEnumerable<Product>>(jsonString);
            }

            return null;
        }
    }
}
