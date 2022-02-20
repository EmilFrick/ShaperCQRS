using MediatR;
using Shaper.Models.Entities;

namespace Shaper.API.CQRS.ShapeData.Commands
{
    public record DeleteShapeCommand(int Id) : IRequest<Shape>;
}
