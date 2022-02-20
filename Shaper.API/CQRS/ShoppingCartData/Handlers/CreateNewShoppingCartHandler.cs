using MediatR;
using Shaper.API.CQRS.ShoppingCartData.Commands;
using Shaper.DataAccess.Context;
using Shaper.Models.Entities;

namespace Shaper.API.CQRS.ShoppingCartData.Handlers
{
    public class CreateNewShoppingCartHandler : IRequestHandler<CreateNewShoppingCartCommand, ShoppingCart>
    {

        private readonly AppDbContext _db;

        public CreateNewShoppingCartHandler(AppDbContext db)
        {
            _db = db;
        }

        public async Task<ShoppingCart> Handle(CreateNewShoppingCartCommand request, CancellationToken cancellationToken)
        {
            ShoppingCart cart = new()
            {
                CustomerIdentity = request.CustomerIdentity,
                CheckedOut = false,
                OrderValue = 0,
            };
            await _db.ShoppingCarts.AddAsync(cart);
            await _db.SaveChangesAsync();
            return cart;
        }
    }
}
