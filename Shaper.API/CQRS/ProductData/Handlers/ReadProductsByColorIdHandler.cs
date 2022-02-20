using MediatR;
using Microsoft.EntityFrameworkCore;
using Shaper.API.CQRS.ProductData.Queries;
using Shaper.DataAccess.Context;
using Shaper.Models.Entities;

namespace Shaper.API.CQRS.ProductData.Handlers
{
    public class ReadProductsByColorIdHandler : IRequestHandler<ReadProductsByColorIdQuery, IEnumerable<Product>>
    {
        private readonly AppDbContext _db;
        private readonly IMediator _mediator;


        public ReadProductsByColorIdHandler(AppDbContext db, IMediator mediator)
        {
            _db = db;
            _mediator = mediator;
        }

        public async Task<IEnumerable<Product>> Handle(ReadProductsByColorIdQuery request, CancellationToken cancellationToken) => await _mediator.Send(new ReadFilteredProductsQuery(x=>x.ColorId == request.Id));
    }
}
