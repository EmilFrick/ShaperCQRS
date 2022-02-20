using MediatR;
using Microsoft.EntityFrameworkCore;
using Shaper.API.CQRS.ProductData.Queries;
using Shaper.DataAccess.Context;
using Shaper.Models.Entities;

namespace Shaper.API.CQRS.ProductData.Handlers
{
    public class ReadProductsByShapeIdHandler : IRequestHandler<ReadProductsByShapeIdQuery, IEnumerable<Product>>
    {
        private readonly AppDbContext _db;
        private readonly IMediator _mediator;


        public ReadProductsByShapeIdHandler(AppDbContext db, IMediator mediator)
        {
            _db = db;
            _mediator = mediator;
        }

        public async Task<IEnumerable<Product>> Handle(ReadProductsByShapeIdQuery request, CancellationToken cancellationToken) => await _mediator.Send(new ReadFilteredProductsQuery(x=>x.ShapeId == request.Id));
    }
}
