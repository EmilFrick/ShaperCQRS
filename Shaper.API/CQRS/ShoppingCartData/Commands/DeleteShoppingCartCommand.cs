using MediatR;
using Shaper.Models.Entities;

namespace Shaper.API.CQRS.ShoppingCartData.Commands
{
    public record DeleteShoppingCartCommand(int id) : IRequest<ShoppingCart>;
}
