using MediatR;
using Microsoft.EntityFrameworkCore;
using Shaper.API.CQRS.OrderData.Queries;
using Shaper.DataAccess.Context;
using Shaper.Models.Entities;

namespace Shaper.API.CQRS.OrderData.Handlers
{
    public class ReadOrdersHandler : IRequestHandler<ReadOrdersQuery, List<Order>>
    {
        private readonly AppDbContext _db;


        public ReadOrdersHandler(AppDbContext db)
        {
            _db = db;
        }

        public async Task<List<Order>> Handle(ReadOrdersQuery request, CancellationToken cancellationToken)
        {
            return await _db.Orders.Include(x=>x.OrderProducts).ToListAsync();
        }
    }
}
