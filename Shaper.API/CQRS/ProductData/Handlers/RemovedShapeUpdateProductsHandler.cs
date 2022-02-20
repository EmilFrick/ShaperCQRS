using MediatR;
using Shaper.API.CQRS.ShapeData.Queries;
using Shaper.API.CQRS.ProductData.Commands;
using Shaper.DataAccess.Context;

namespace Shaper.API.CQRS.ProductData.Handlers
{
    public class RemovedShapeUpdateProductsHandler : IRequestHandler<RemovedShapeUpdateProductsCommand, Task>
    {
        private readonly AppDbContext _db;
        private readonly IMediator _mediator;

        public RemovedShapeUpdateProductsHandler(AppDbContext db, IMediator mediator)
        {
            _db = db;
            _mediator = mediator;
        }

        public async Task<Task> Handle(RemovedShapeUpdateProductsCommand request, CancellationToken cancellationToken)
        {
            var defaultShape = await _mediator.Send(new ReadDefaultShapeQuery());
            var deletedShape = await _mediator.Send(new ReadShapeQuery(x => x.Id == request.RemovedShapeId));
            deletedShape.Products.ToList().ForEach(p => p.Shape = defaultShape);
            _db.Update(deletedShape);
            await _db.SaveChangesAsync();
            await _mediator.Send(new AdjustProductPricesCommand(x => x.ShapeId == defaultShape.Id));
            return Task.CompletedTask;
        }
    }
}
