using MediatR;
using System.Linq.Expressions;
using ShoppingCart = Shaper.Models.Entities.ShoppingCart;

namespace Shaper.API.CQRS.ShoppingCartData.Queries
{
    public record ReadShoppingCartsQuery() : IRequest<List<ShoppingCart>>;
}
