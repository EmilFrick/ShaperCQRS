using MediatR;
using System.Linq.Expressions;
using Product = Shaper.Models.Entities.Product;

namespace Shaper.API.CQRS.ProductData.Queries
{
    public record ReadProductsQuery() : IRequest<List<Product>>;
}
