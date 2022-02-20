using MediatR;
using Shaper.API.CQRS.ColorData.Queries;
using Shaper.API.CQRS.ProductData.Commands;
using Shaper.DataAccess.Context;

namespace Shaper.API.CQRS.ProductData.Handlers
{
    public class RemovedColorUpdateProductsHandler : IRequestHandler<RemovedColorUpdateProductsCommand, Task>
    {
        private readonly AppDbContext _db;
        private readonly IMediator _mediator;

        public RemovedColorUpdateProductsHandler(AppDbContext db, IMediator mediator)
        {
            _db = db;
            _mediator = mediator;
        }

        public async Task<Task> Handle(RemovedColorUpdateProductsCommand request, CancellationToken cancellationToken)
        {
            var defaultColor = await _mediator.Send(new ReadDefaultColorQuery());
            var deletedColor = await _mediator.Send(new ReadColorQuery(x => x.Id == request.RemovedColorId));
            deletedColor.Products.ToList().ForEach(p => p.Color = defaultColor);
            _db.Update(deletedColor);
            await _db.SaveChangesAsync();
            await _mediator.Send(new AdjustProductPricesCommand(x => x.ColorId == defaultColor.Id));
            return Task.CompletedTask;
        }
    }
}
