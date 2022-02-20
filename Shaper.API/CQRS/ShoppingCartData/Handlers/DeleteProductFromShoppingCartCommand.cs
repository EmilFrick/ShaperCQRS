using MediatR;
using Microsoft.EntityFrameworkCore;
using Shaper.API.CQRS.ShoppingCartData.Commands;
using Shaper.DataAccess.Context;
using Shaper.Models.Entities;

namespace Shaper.API.CQRS.ShoppingCartData.Handlers
{
    public class DeleteProductFromShoppingCartHandler : IRequestHandler<DeleteProductFromShoppingCartCommand, Task>
    {

        private readonly AppDbContext _db;

        public DeleteProductFromShoppingCartHandler(AppDbContext db)
        {
            _db = db;
        }

        public async Task<Task> Handle(DeleteProductFromShoppingCartCommand request, CancellationToken cancellationToken)
        {
            var itemToDelete = await _db.CartProducts.FirstOrDefaultAsync(a => a.ShoppingCartId == request.ShoppingCartId && a.ProductId == request.ProductId);
            _db.CartProducts.Remove(itemToDelete);
            await _db.SaveChangesAsync();
            return Task.CompletedTask;
        }
    }
}
