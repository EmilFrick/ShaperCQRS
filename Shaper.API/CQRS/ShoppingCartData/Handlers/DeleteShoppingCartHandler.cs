using MediatR;
using Microsoft.EntityFrameworkCore;
using Shaper.API.CQRS.ShoppingCartData.Commands;
using Shaper.DataAccess.Context;
using Shaper.Models.Entities;

namespace Shaper.API.CQRS.ShoppingCartData.Handlers
{
    public class DeleteShoppingCartHandler : IRequestHandler<DeleteShoppingCartCommand, ShoppingCart>
    {

        private readonly AppDbContext _db;

        public DeleteShoppingCartHandler(AppDbContext db)
        {
            _db = db;
        }

        public async Task<ShoppingCart> Handle(DeleteShoppingCartCommand request, CancellationToken cancellationToken)
        {
            var cart = await _db.ShoppingCarts.FirstOrDefaultAsync(x => x.Id == request.id);
            if (cart is null)
                return null;

            _db.ShoppingCarts.Remove(cart);
            await _db.SaveChangesAsync();
            return cart;
        }
    }
}
