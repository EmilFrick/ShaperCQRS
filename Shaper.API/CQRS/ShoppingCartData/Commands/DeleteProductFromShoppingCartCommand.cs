using MediatR;
using Shaper.Models.Entities;

namespace Shaper.API.CQRS.ShoppingCartData.Commands
{
    public record DeleteProductFromShoppingCartCommand(int ShoppingCartId, int ProductId) : IRequest<Task>;
}
