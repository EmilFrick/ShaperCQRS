using MediatR;
using Microsoft.EntityFrameworkCore;
using Shaper.API.CQRS.OrderData.Queries;
using Shaper.DataAccess.Context;
using Shaper.Models.Entities;

namespace Shaper.API.CQRS.OrderData.Handlers
{
    public class ReadOrderHandler : IRequestHandler<ReadOrderQuery, Order>
    {
        private readonly AppDbContext _db;

        public ReadOrderHandler(AppDbContext db)
        {
            _db = db;
        }

        public async Task<Order> Handle(ReadOrderQuery request, CancellationToken cancellationToken)
        {
            return await _db.Orders.Include(x => x.OrderProducts).FirstOrDefaultAsync(request.filter);
        }
    }
}
