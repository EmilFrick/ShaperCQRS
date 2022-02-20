using MediatR;
using Shaper.Models.Entities;

namespace Shaper.API.CQRS.ColorData.Queries
{
    public record ReadDefaultColorQuery() : IRequest<Color>;
    
    
}
