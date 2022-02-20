using MediatR;
using Shaper.Models.Entities;
using System.Linq.Expressions;

namespace Shaper.API.CQRS.ShoppingCartData.Queries
{
    public record ReadFilteredShoppingCartsQuery(Expression<Func<ShoppingCart,bool>>? filter) : IRequest<List<ShoppingCart>>;
}
