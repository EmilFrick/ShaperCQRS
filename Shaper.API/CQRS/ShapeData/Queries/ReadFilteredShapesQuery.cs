using MediatR;
using System.Linq.Expressions;
using Shape = Shaper.Models.Entities.Shape;

namespace Shaper.API.CQRS.ShapeData.Queries
{
    public record ReadFilteredShapesQuery(Expression<Func<Shape,bool>>? filter) : IRequest<List<Shape>>;
}
