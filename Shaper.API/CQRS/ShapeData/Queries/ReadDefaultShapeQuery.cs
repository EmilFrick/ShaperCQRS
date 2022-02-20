using MediatR;
using Shaper.Models.Entities;

namespace Shaper.API.CQRS.ShapeData.Queries
{
    public record ReadDefaultShapeQuery() : IRequest<Shape>;
    
    
}
