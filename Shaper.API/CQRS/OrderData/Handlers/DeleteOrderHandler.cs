using MediatR;
using Microsoft.EntityFrameworkCore;
using Shaper.API.CQRS.OrderData.Commands;
using Shaper.DataAccess.Context;
using Shaper.Models.Entities;

namespace Shaper.API.CQRS.OrderData.Handlers
{
    public class DeleteOrderHandler : IRequestHandler<DeleteOrderCommand, Order>
    {

        private readonly AppDbContext _db;

        public DeleteOrderHandler(AppDbContext db)
        {
            _db = db;
        }

        public async Task<Order> Handle(DeleteOrderCommand request, CancellationToken cancellationToken)
        {
            var order = await _db.Orders.FirstOrDefaultAsync(x => x.Id == request.id);
            _db.Remove(order);
            await _db.SaveChangesAsync();
            return order;
        }
    }
}
