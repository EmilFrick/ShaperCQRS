using MediatR;
using Shaper.API.CQRS.ShoppingCartData.Commands;
using Shaper.DataAccess.Context;
using Shaper.Models.Entities;
using Shaper.Models.Models.ProductModels;

namespace Shaper.API.CQRS.ShoppingCartData.Handlers
{
    public class AddProductToShoppingCartHandler : IRequestHandler<AddProductToShoppingCartCommand, CartProduct>
    {

        private readonly AppDbContext _db;

        public AddProductToShoppingCartHandler(AppDbContext db)
        {
            _db = db;
        }

        public async Task<CartProduct> Handle(AddProductToShoppingCartCommand request, CancellationToken cancellationToken)
        {
            CartProduct product = new()
{
                ProductId = request.CartProductModel.ProductId,
                ProductQuantity = request.CartProductModel.ProductQuantity,
                ShoppingCartId = request.ShoppingCartId,
                UnitPrice = request.ProductPrice
            };
            await _db.CartProducts.AddAsync(product);
            await _db.SaveChangesAsync();
            return product;
        }
    }
}
