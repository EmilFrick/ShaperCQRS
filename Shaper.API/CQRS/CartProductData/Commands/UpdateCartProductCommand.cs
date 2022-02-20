using MediatR;
using Shaper.Models.Entities;


namespace Shaper.API.CQRS.CartProductData.Commands
{
    public record UpdateCartProductCommand(CartProduct product, int quantityUpdate) : IRequest<CartProduct>;
}
