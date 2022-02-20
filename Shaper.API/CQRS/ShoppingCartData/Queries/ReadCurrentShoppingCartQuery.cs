using MediatR;
using Shaper.Models.Entities;
using System.Linq.Expressions;

namespace Shaper.API.CQRS.ShoppingCartData.Queries
{
    public record ReadCurrentShoppingCartQuery(string UserIdentity) : IRequest<ShoppingCart>;
}
