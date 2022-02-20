using MediatR;
using Shaper.Models.Entities;
using System.Linq.Expressions;

namespace Shaper.API.CQRS.ShapeData.Queries
{
    public record ReadShapeQuery(Expression<Func<Shape,bool>> filter) : IRequest<Shape>;
}
