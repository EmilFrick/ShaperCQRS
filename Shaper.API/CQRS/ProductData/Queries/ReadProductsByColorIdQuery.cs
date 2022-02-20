using MediatR;
using Shaper.Models.Entities;
using System.Linq.Expressions;

namespace Shaper.API.CQRS.ProductData.Queries
{
    public record ReadProductsByColorIdQuery(int Id) : IRequest<IEnumerable<Product>>;
}
