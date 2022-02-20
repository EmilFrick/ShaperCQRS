using MediatR;
using Shaper.API.CQRS.ColorData.Commands;
using Shaper.API.CQRS.OrderData.Commands;
using Shaper.DataAccess.Context;
using Shaper.Models.Entities;

namespace Shaper.API.CQRS.OrderData.Handlers
{
    public class CreateOrderHandler : IRequestHandler<CreateOrderCommand, Order>
    {

        private readonly AppDbContext _db;
        private readonly IMediator _mediator;

        public CreateOrderHandler(AppDbContext db, IMediator mediator)
        {
            _db = db;
            _mediator = mediator;
        }

        public async Task<Order> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
        {
            var order = await _mediator.Send(new InitiateOrderCommand(request.UserShoppingCart.CustomerIdentity));
            await _mediator.Send(new CheckoutProductsCommand(request.UserShoppingCart, order));
            await _mediator.Send(new CalculateOrderCommand(order.Id));
            await _mediator.Send(new CheckoutShoppingCartCommand(request.UserShoppingCart.CustomerIdentity));
            return order;
        }
    }
}
