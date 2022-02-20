using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shaper.Models.Entities;
using Shaper.Models.Models.ProductComponentsModels;
using System.Linq.Expressions;

namespace Shaper.API.CQRS.ColorData.Queries
{
    public record ReadProductComponentsQuery(ProductReqComponentsModel ComponentsRequest) : IRequest<ProductResComponentsModel>;
}
