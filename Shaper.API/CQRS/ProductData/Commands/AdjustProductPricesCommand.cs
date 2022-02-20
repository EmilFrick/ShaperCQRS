using MediatR;
using Shaper.Models.Entities;
using System.Linq.Expressions;

namespace Shaper.API.CQRS.ProductData.Commands
{
    public record AdjustProductPricesCommand(Expression<Func<Product, bool>> Filter) : IRequest<Task>;
}
