using MediatR;
using Microsoft.EntityFrameworkCore;
using Shaper.API.CQRS.ShoppingCartData.Queries;
using Shaper.DataAccess.Context;
using Shaper.Models.Entities;

namespace Shaper.API.CQRS.ShoppingCartData.Handlers
{
    public class ReadShoppingCartHandler : IRequestHandler<ReadShoppingCartQuery, ShoppingCart>
    {
        private readonly AppDbContext _db;

        public ReadShoppingCartHandler(AppDbContext db)
        {
            _db = db;
        }

        public async Task<ShoppingCart> Handle(ReadShoppingCartQuery request, CancellationToken cancellationToken)
        {
            return await _db.ShoppingCarts.Include(x => x.CartProducts).FirstOrDefaultAsync(request.filter);
        }
    }
}
