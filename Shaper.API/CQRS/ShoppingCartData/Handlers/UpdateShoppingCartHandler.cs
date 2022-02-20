using MediatR;
using Shaper.API.CQRS.ProductData.Queries;
using Shaper.API.CQRS.ShoppingCartData.Commands;
using Shaper.API.CQRS.ShoppingCartData.Queries;
using Shaper.DataAccess.Context;
using Shaper.Models.Entities;
using System.Web.Helpers;
using ShoppingCart = Shaper.Models.Entities.ShoppingCart;


namespace Shaper.API.CQRS.ShoppingCartData.Handlers
{
    public class UpdateShoppingCartHandler : IRequestHandler<UpdateShoppingCartCommand, ShoppingCart>
    {
        private readonly AppDbContext _db;
        private readonly IMediator _mediator;



        public UpdateShoppingCartHandler(AppDbContext db, IMediator mediator)
        {
            _db = db;
            _mediator = mediator;
        }

        public async Task<ShoppingCart> Handle(UpdateShoppingCartCommand request, CancellationToken cancellationToken)
        {
            var originalShoppingcart = await _mediator.Send(new ReadShoppingCartQuery(x => x.Id == request.Id));
            if (request.Model.CartProducts != null && request.Model.CartProducts.Count > 0)
            {
                List<CartProduct> cartProducts = new();
                CartProduct cartProduct = new();
                double sum = 0;
                foreach (var item in request.Model.CartProducts)
                {
                    var productdetails = await _mediator.Send(new ReadProductQuery(x => x.Id == item.ProductId));
                    cartProduct = new()
                    {
                        ProductId = item.ProductId,
                        ProductQuantity = item.Quantity,
                        UnitPrice = productdetails.Price
                    };
                    sum += (cartProduct.UnitPrice * cartProduct.ProductQuantity);
                    cartProducts.Add(cartProduct);
                }
                originalShoppingcart.OrderValue = sum;
                originalShoppingcart.CartProducts = cartProducts;
            }

            originalShoppingcart.CustomerIdentity = request.Model.UserIdentity;
            originalShoppingcart.CheckedOut = request.Model.CheckedOut.GetValueOrDefault();
            _db.ShoppingCarts.Update(originalShoppingcart);
            await _db.SaveChangesAsync();
            return originalShoppingcart;
        }
    }
}
