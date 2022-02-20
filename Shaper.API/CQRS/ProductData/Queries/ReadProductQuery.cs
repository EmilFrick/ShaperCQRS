using MediatR;
using Shaper.Models.Entities;
using System.Linq.Expressions;

namespace Shaper.API.CQRS.ProductData.Queries
{
    public record ReadProductQuery(Expression<Func<Product,bool>> filter) : IRequest<Product>;
}
