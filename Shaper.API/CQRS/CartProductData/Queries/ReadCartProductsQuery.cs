using MediatR;
using System.Linq.Expressions;
using CartProduct = Shaper.Models.Entities.CartProduct;

namespace Shaper.API.CQRS.CartProductData.Queries
{
    public record ReadCartProductsQuery() : IRequest<List<CartProduct>>;
}
