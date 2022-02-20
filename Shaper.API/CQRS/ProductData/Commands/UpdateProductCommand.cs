using MediatR;
using Shaper.Models.Entities;
using Shaper.Models.Models.ProductModels;

namespace Shaper.API.CQRS.ProductData.Commands
{
    public record UpdateProductCommand(ProductUpdateModel Model) : IRequest<Product>;
}
