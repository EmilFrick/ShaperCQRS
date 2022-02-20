using MediatR;
using Microsoft.EntityFrameworkCore;
using Shaper.API.CQRS.OrderData.Queries;
using Shaper.DataAccess.Context;
using Shaper.Models.Entities;

namespace Shaper.API.CQRS.OrderData.Handlers
{
    public class GetOrdersHandler : IRequestHandler<ReadFilteredOrdersQuery, List<Order>>
    {
        private readonly AppDbContext _db;


        public GetOrdersHandler(AppDbContext db)
        {
            _db = db;
        }

        public async Task<List<Order>> Handle(ReadFilteredOrdersQuery request, CancellationToken cancellationToken)
        {
            return await _db.Orders.Include(x=>x.OrderProducts).Where(request.filter).ToListAsync();
        }
    }
}
