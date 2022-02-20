using MediatR;

namespace Shaper.API.CQRS.ProductData.Commands
{
    public record RemovedColorUpdateProductsCommand(int RemovedColorId) : IRequest<Task>;
}
