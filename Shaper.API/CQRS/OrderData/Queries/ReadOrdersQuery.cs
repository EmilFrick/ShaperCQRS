using MediatR;
using System.Linq.Expressions;
using Order = Shaper.Models.Entities.Order;

namespace Shaper.API.CQRS.OrderData.Queries
{
    public record ReadOrdersQuery() : IRequest<List<Order>>;
}
