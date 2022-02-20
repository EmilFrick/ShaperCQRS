using MediatR;
using Shaper.API.CQRS.TransparencyData.Queries;
using Shaper.API.CQRS.ProductData.Commands;
using Shaper.DataAccess.Context;

namespace Shaper.API.CQRS.ProductData.Handlers
{
    public class RemovedTransparencyUpdateProductsHandler : IRequestHandler<RemovedTransparencyUpdateProductsCommand, Task>
    {
        private readonly AppDbContext _db;
        private readonly IMediator _mediator;

        public RemovedTransparencyUpdateProductsHandler(AppDbContext db, IMediator mediator)
        {
            _db = db;
            _mediator = mediator;
        }

        public async Task<Task> Handle(RemovedTransparencyUpdateProductsCommand request, CancellationToken cancellationToken)
        {
            var defaultTransparency = await _mediator.Send(new ReadDefaultTransparencyQuery());
            var deletedTransparency = await _mediator.Send(new ReadTransparencyQuery(x => x.Id == request.RemovedTransparencyId));
            deletedTransparency.Products.ToList().ForEach(p => p.Transparency = defaultTransparency);
            _db.Update(deletedTransparency);
            await _db.SaveChangesAsync();
            await _mediator.Send(new AdjustProductPricesCommand(x => x.TransparencyId == defaultTransparency.Id));
            return Task.CompletedTask;
        }
    }
}
