using MediatR;
using Shaper.Models.Entities;
using Shaper.Models.Models.ShapeModels;

namespace Shaper.API.CQRS.ShapeData.Commands
{
    public record CreateShapeCommand(ShapeCreateModel ShapeCreateModel) : IRequest<Shape>;  
    
}
