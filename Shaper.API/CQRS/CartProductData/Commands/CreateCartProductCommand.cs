using MediatR;
using Shaper.Models.Entities;

namespace Shaper.API.CQRS.CartProductData.Commands
{
    public record CreateCartProductCommand() : IRequest<CartProduct>;
    
}
