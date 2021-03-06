using MediatR;
using Shaper.Models.Entities;
using System.Linq.Expressions;

namespace Shaper.API.CQRS.CartProductData.Queries
{
    public record ReadCartProductQuery(Expression<Func<CartProduct,bool>> filter) : IRequest<CartProduct>;
}
