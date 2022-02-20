using MediatR;
using Microsoft.EntityFrameworkCore;
using Shaper.API.CQRS.ShoppingCartData.Queries;
using Shaper.DataAccess.Context;
using Shaper.Models.Entities;

namespace Shaper.API.CQRS.ShoppingCartData.Handlers
{
    public class ReadCurrentShoppingCartHandler : IRequestHandler<ReadCurrentShoppingCartQuery, ShoppingCart>
    {
        private readonly AppDbContext _db;

        public ReadCurrentShoppingCartHandler(AppDbContext db)
        {
            _db = db;
        }

        public async Task<ShoppingCart> Handle(ReadCurrentShoppingCartQuery request, CancellationToken cancellationToken)
        {
            return await _db.ShoppingCarts.Include(x => x.CartProducts).FirstOrDefaultAsync(x => x.CustomerIdentity == request.UserIdentity && x.CheckedOut == false);
        }
    }
}
