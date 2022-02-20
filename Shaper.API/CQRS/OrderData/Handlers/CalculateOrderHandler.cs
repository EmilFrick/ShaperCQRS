using MediatR;
using Shaper.API.CQRS.ColorData.Commands;
using Shaper.API.CQRS.OrderData.Commands;
using Shaper.API.CQRS.OrderData.Queries;
using Shaper.DataAccess.Context;
using Shaper.Models.Entities;

namespace Shaper.API.CQRS.OrderData.Handlers
{
    public class CalculateOrderHandler : IRequestHandler<CalculateOrderCommand, Order>
    {

        private readonly AppDbContext _db;
        private readonly IMediator _mediator;

        public CalculateOrderHandler(AppDbContext db, IMediator mediator)
        {
            _db = db;
            _mediator = mediator;
        }

        public async Task<Order> Handle(CalculateOrderCommand request, CancellationToken cancellationToken)
        {
            double orderTotalValue = 0;
            var order = await _mediator.Send(new ReadOrderQuery(x => x.Id == request.OrderId));
            foreach (var item in order.OrderProducts)
            {
                orderTotalValue += item.EntryTotalValue;
            }
            order.OrderValue = orderTotalValue;
            _db.Orders.Update(order);
            await _db.SaveChangesAsync();
            return order;
        }
    }
}
