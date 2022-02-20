using MediatR;

namespace Shaper.API.CQRS.ProductData.Commands
{
    public record RemovedShapeUpdateProductsCommand(int RemovedShapeId) : IRequest<Task>;
}
