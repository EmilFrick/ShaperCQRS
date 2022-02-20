using MediatR;
using Shaper.API.CQRS.ColorData.Commands;
using Shaper.DataAccess.Context;
using Shaper.Models.Entities;

namespace Shaper.API.CQRS.ColorData.Handlers
{
    public class InitiateOrderHandler : IRequestHandler<InitiateOrderCommand, Order>
    {

        private readonly AppDbContext _db;

        public InitiateOrderHandler(AppDbContext db)
        {
            _db = db;
        }

        public async Task<Order> Handle(InitiateOrderCommand request, CancellationToken cancellationToken)
        {
            Order order = new() { CustomerIdentity = request.UserIdentity };
            await _db.Orders.AddAsync(order);
            await _db.SaveChangesAsync();
            return order;
        }
    }
}
