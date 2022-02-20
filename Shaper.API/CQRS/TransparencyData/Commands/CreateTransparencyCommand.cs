using MediatR;
using Shaper.Models.Entities;
using Shaper.Models.Models.TransparencyModels;

namespace Shaper.API.CQRS.TransparencyData.Commands
{
    public record CreateTransparencyCommand(TransparencyCreateModel TransparencyCreateModel) : IRequest<Transparency>;
    
}
