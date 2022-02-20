using MediatR;
using Shaper.API.CQRS.OrderData.Commands;
using Shaper.API.CQRS.OrderData.Queries;
using Shaper.DataAccess.Context;
using Shaper.Models.Entities;
using Shaper.Models.Models.OrderModels;
using Order = Shaper.Models.Entities.Order;


namespace Shaper.API.CQRS.OrderData.Handlers
{
    public class UpdateOrderHandler : IRequestHandler<UpdateOrderCommand, Order>
    {
        private readonly AppDbContext _db;
        private readonly IMediator _mediator;



        public UpdateOrderHandler(AppDbContext db, IMediator mediator)
        {
            _db = db;
            _mediator = mediator;
        }

        public async Task<Order> Handle(UpdateOrderCommand request, CancellationToken cancellationToken)
        {
            request.OriginalOrder.CustomerIdentity = request.UpdatedOrderModel.CustomerIdentity is not null ? request.UpdatedOrderModel.CustomerIdentity  : request.OriginalOrder.CustomerIdentity;
            if (request.UpdatedOrderModel.OrderEntries is not null)
                await _mediator.Send(new UpdateOrderDetailsCommand(request.UpdatedOrderModel.OrderEntries, request.OriginalOrder.OrderProducts.ToList()));

            _db.Orders.Update(request.OriginalOrder);
            await _db.SaveChangesAsync();
            return request.OriginalOrder;
        }

    }
}
