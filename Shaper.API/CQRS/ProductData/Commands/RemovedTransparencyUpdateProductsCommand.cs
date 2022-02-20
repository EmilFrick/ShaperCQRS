using MediatR;

namespace Shaper.API.CQRS.ProductData.Commands
{
    public record RemovedTransparencyUpdateProductsCommand(int RemovedTransparencyId) : IRequest<Task>;
}
