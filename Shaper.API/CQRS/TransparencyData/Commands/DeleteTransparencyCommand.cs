using MediatR;
using Shaper.Models.Entities;

namespace Shaper.API.CQRS.TransparencyData.Commands
{
    public record DeleteTransparencyCommand(int Id) : IRequest<Transparency>;
}
