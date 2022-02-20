using MediatR;
using Shaper.Models.Entities;
using Shaper.Models.Models.OrderModels;

namespace Shaper.API.CQRS.OrderData.Commands
{
    public record UpdateOrderDetailsCommand(List<OrderDetailModel> updatedOrderEntries, List<OrderDetail> originalOrderEntries) : IRequest<List<OrderDetail>>;
}
