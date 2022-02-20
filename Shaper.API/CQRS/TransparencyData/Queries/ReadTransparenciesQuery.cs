using MediatR;
using System.Linq.Expressions;
using Transparency = Shaper.Models.Entities.Transparency;

namespace Shaper.API.CQRS.TransparencyData.Queries
{
    public record ReadTransparenciesQuery() : IRequest<List<Transparency>>;
}
