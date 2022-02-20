using MediatR;
using Shaper.Models.Entities;

namespace Shaper.API.CQRS.CartProductData.Commands
{
    public record DeleteCartProductCommand(int id) : IRequest<CartProduct>;
}
