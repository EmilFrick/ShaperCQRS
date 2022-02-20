using MediatR;
using Shaper.Models.Entities;
using System.Linq.Expressions;

namespace Shaper.API.CQRS.ShoppingCartData.Queries
{
    public record ReadShoppingCartQuery(Expression<Func<ShoppingCart,bool>> filter) : IRequest<ShoppingCart>;
}
