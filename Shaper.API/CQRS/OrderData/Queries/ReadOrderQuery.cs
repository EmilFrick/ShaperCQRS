using MediatR;
using Shaper.Models.Entities;
using System.Linq.Expressions;

namespace Shaper.API.CQRS.OrderData.Queries
{
    public record ReadOrderQuery(Expression<Func<Order,bool>> filter) : IRequest<Order>;
}
