using MediatR;
using Shaper.Models.Entities;

namespace Shaper.API.CQRS.ProductData.Commands
{
    public record DeleteProductCommand(int id) : IRequest<Product>;
}
