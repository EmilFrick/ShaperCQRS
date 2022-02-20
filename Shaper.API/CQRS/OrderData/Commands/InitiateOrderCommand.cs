using MediatR;
using Shaper.Models.Entities;
using Shaper.Models.Models.ColorModels;

namespace Shaper.API.CQRS.ColorData.Commands
{
    public record InitiateOrderCommand(string UserIdentity) : IRequest<Order>;
    
}
