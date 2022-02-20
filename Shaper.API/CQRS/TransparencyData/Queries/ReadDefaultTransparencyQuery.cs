using MediatR;
using Shaper.Models.Entities;

namespace Shaper.API.CQRS.TransparencyData.Queries
{
    public record ReadDefaultTransparencyQuery() : IRequest<Transparency>;
    
    
}
