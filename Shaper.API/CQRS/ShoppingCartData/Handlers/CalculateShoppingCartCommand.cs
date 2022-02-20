using MediatR;
using Shaper.API.CQRS.ShoppingCartData.Commands;
using Shaper.API.CQRS.ShoppingCartData.Queries;
using Shaper.DataAccess.Context;
using System.Web.Helpers;
using ShoppingCart = Shaper.Models.Entities.ShoppingCart;


namespace Shaper.API.CQRS.ShoppingCartData.Handlers
{
    public class CalculateShoppingCartHandler : IRequestHandler<CalculateShoppingCartCommand, ShoppingCart>
    {
        private readonly AppDbContext _db;
        private readonly IMediator _mediator;



        public CalculateShoppingCartHandler(AppDbContext db, IMediator mediator)
        {
            _db = db;
            _mediator = mediator;
        }

        public async Task<ShoppingCart> Handle(CalculateShoppingCartCommand request, CancellationToken cancellationToken)
        {
            double cartValue = 0;
            foreach (var item in request.ShoppingCart.CartProducts)
            {
                cartValue += (item.ProductQuantity * item.UnitPrice);
            }
            request.ShoppingCart.OrderValue = cartValue;
            _db.ShoppingCarts.Update(request.ShoppingCart);
            await _db.SaveChangesAsync();
            return request.ShoppingCart;
        }
    }
}
