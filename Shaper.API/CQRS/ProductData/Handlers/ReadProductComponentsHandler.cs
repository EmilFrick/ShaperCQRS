using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shaper.API.CQRS.ColorData.Queries;
using Shaper.API.CQRS.ShapeData.Queries;
using Shaper.API.CQRS.TransparencyData.Queries;
using Shaper.DataAccess.Context;
using Shaper.Models.Entities;
using Shaper.Models.Models.ProductComponentsModels;

namespace Shaper.API.CQRS.ColorData.Handlers
{
    public class ReadProductComponentsHandler : IRequestHandler<ReadProductComponentsQuery, ProductResComponentsModel>
    {
        private readonly AppDbContext _db;
        private readonly IMediator _mediator;

        public ReadProductComponentsHandler(AppDbContext db, IMediator mediator)
        {
            _db = db;
            _mediator = mediator;
        }

        public async Task<ProductResComponentsModel> Handle(ReadProductComponentsQuery request, CancellationToken cancellationToken)
        {
            ProductResComponentsModel requestResult = new();
            requestResult.ColorComponent = await _mediator.Send(new ReadColorQuery(x => x.Id == request.ComponentsRequest.ColorId));
            requestResult.ShapeComponent = await _mediator.Send(new ReadShapeQuery(x => x.Id == request.ComponentsRequest.ShapeId));
            requestResult.TransparencyComponent = await _mediator.Send(new ReadTransparencyQuery(x => x.Id == request.ComponentsRequest.TransparencyId));

            return requestResult;
        }
    }
}
