using MediatR;
using Shaper.API.CQRS.ShoppingCartData.Commands;
using Shaper.DataAccess.Context;
using Shaper.Models.Entities;

namespace Shaper.API.CQRS.ShoppingCartData.Handlers
{
    public class CreateShoppingCartHandler : IRequestHandler<CreateShoppingCartCommand, ShoppingCart>
    {

        private readonly AppDbContext _db;

        public CreateShoppingCartHandler(AppDbContext db)
        {
            _db = db;
        }

        public async Task<ShoppingCart> Handle(CreateShoppingCartCommand request, CancellationToken cancellationToken)
        {
            var shoppingCart = new ShoppingCart();
            await _db.AddAsync(shoppingCart);
            await _db.SaveChangesAsync();
            return shoppingCart;
        }
    }
}
