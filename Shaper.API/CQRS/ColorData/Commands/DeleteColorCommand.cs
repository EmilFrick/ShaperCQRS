using MediatR;
using Shaper.Models.Entities;

namespace Shaper.API.CQRS.ColorData.Commands
{
    public record DeleteColorCommand(int id) : IRequest<Color>;
}
