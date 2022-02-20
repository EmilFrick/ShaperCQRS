using MediatR;
using Shaper.Models.Entities;
using System.Linq.Expressions;

namespace Shaper.API.CQRS.ProductData.Queries
{
    public record ReadProductsByTransparencyIdQuery(int Id) : IRequest<IEnumerable<Product>>;
}
