using MediatR;
using Microsoft.EntityFrameworkCore;
using Shaper.API.CQRS.ShoppingCartData.Queries;
using Shaper.DataAccess.Context;
using Shaper.Models.Entities;

namespace Shaper.API.CQRS.ShoppingCartData.Handlers
{
    public class GetShoppingCartsHandler : IRequestHandler<ReadFilteredShoppingCartsQuery, List<ShoppingCart>>
    {
        private readonly AppDbContext _db;


        public GetShoppingCartsHandler(AppDbContext db)
        {
            _db = db;
        }

        public async Task<List<ShoppingCart>> Handle(ReadFilteredShoppingCartsQuery request, CancellationToken cancellationToken)
        {
            return await _db.ShoppingCarts.Include(x=>x.CartProducts).Where(request.filter).ToListAsync();
        }
    }
}
