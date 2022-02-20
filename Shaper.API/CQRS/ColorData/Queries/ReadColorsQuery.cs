using MediatR;
using System.Linq.Expressions;
using Color = Shaper.Models.Entities.Color;

namespace Shaper.API.CQRS.ColorData.Queries
{
    public record ReadColorsQuery() : IRequest<List<Color>>;
}
