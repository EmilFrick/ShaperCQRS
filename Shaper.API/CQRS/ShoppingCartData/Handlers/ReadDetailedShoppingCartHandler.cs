using MediatR;
using Microsoft.EntityFrameworkCore;
using Shaper.API.CQRS.ColorData.Queries;
using Shaper.API.CQRS.ShoppingCartData.Queries;
using Shaper.DataAccess.Context;
using Shaper.Models.Entities;

namespace Shaper.API.CQRS.ShoppingCartData.Handlers
{
    public class ReadDetailedShoppingCartHandler : IRequestHandler<ReadDetailedShoppingCartQuery, ShoppingCart>
    {
        private readonly AppDbContext _db;

        public ReadDetailedShoppingCartHandler(AppDbContext db)
        {
            _db = db;
        }

        public async Task<ShoppingCart> Handle(ReadDetailedShoppingCartQuery request, CancellationToken cancellationToken)
        {
            var cart = await _db.ShoppingCarts.FirstOrDefaultAsync(y => y.CustomerIdentity == request.userIdentity && y.CheckedOut == false);
            var cartProductsResult = await _db.CartProducts.Include(p => p.Product)
                                                                .ThenInclude(c => c.Color)
                                                             .Include(p => p.Product)
                                                             .ThenInclude(s => s.Shape)
                                                             .Include(p => p.Product)
                                                                .ThenInclude(t => t.Transparency)
                                                             .Include(sc => sc.ShoppingCart)
                                                                 .Where(x => x.ShoppingCart.CartProducts
                                                                    .Any(y => y.ShoppingCart.CustomerIdentity == request.userIdentity && y.ShoppingCart.CheckedOut == false))
                                                             .ToListAsync();
            List<CartProduct> cartProducts = new List<CartProduct>();
            foreach (var item in cartProductsResult)
            {
                cartProducts.Add(item);
            }
            if(cartProductsResult.Count > 0)
                cart.CartProducts = cartProductsResult;                

            return cart;
        }
    }
}
