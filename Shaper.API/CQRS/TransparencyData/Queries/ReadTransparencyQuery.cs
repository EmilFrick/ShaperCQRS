using MediatR;
using Shaper.Models.Entities;
using System.Linq.Expressions;

namespace Shaper.API.CQRS.TransparencyData.Queries
{
    public record ReadTransparencyQuery(Expression<Func<Transparency,bool>> filter) : IRequest<Transparency>;
}
