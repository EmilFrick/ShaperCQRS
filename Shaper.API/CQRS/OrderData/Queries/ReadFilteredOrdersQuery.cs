using MediatR;
using Shaper.Models.Entities;
using System.Linq.Expressions;

namespace Shaper.API.CQRS.OrderData.Queries
{
    public record ReadFilteredOrdersQuery(Expression<Func<Order,bool>>? filter) : IRequest<List<Order>>;
}
