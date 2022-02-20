using MediatR;
using Microsoft.EntityFrameworkCore;
using Shaper.API.CQRS.ShoppingCartData.Queries;
using Shaper.DataAccess.Context;
using Shaper.Models.Entities;

namespace Shaper.API.CQRS.ShoppingCartData.Handlers
{
    public class ReadShoppingCartsHandler : IRequestHandler<ReadShoppingCartsQuery, List<ShoppingCart>>
    {
        private readonly AppDbContext _db;


        public ReadShoppingCartsHandler(AppDbContext db)
        {
            _db = db;
        }

        public async Task<List<ShoppingCart>> Handle(ReadShoppingCartsQuery request, CancellationToken cancellationToken)
        {
            return await _db.ShoppingCarts.Include(x=>x.CartProducts).ToListAsync();
        }
    }
}
