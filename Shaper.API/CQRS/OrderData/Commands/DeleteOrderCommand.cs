using MediatR;
using Shaper.Models.Entities;

namespace Shaper.API.CQRS.OrderData.Commands
{
    public record DeleteOrderCommand(int id) : IRequest<Order>;
}
