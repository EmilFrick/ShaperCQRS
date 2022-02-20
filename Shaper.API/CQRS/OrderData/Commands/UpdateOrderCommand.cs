using MediatR;
using Shaper.Models.Entities;
using Shaper.Models.Models.OrderModels;

namespace Shaper.API.CQRS.OrderData.Commands
{
    public record UpdateOrderCommand(OrderUpdateModel UpdatedOrderModel, Order OriginalOrder) : IRequest<Order>;
}
