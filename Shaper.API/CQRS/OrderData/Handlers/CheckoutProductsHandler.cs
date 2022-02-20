using MediatR;
using Shaper.API.CQRS.OrderData.Commands;
using Shaper.DataAccess.Context;
using Shaper.Models.Entities;
using System.Web.Helpers;

namespace Shaper.API.CQRS.OrderData.Handlers
{
    public class CheckoutProductsHandler : IRequestHandler<CheckoutProductsCommand, Task>
    {

        private readonly AppDbContext _db;
        private readonly IMediator _mediator;

        public CheckoutProductsHandler(AppDbContext db, IMediator mediator)
        {
            _db = db;
            _mediator = mediator;
        }

        public async Task<Task> Handle(CheckoutProductsCommand request, CancellationToken cancellationToken)
        {
            double orderTotalValue = 0;
            List<OrderDetail> orderDetails = new();
            foreach (var item in request.UserShoppingCart.CartProducts)
            {
                OrderDetail detail = new()
                {
                    OrderId = request.UserOrder.Id,
                    ProductName = item.Product.Name,
                    ProductId = item.ProductId,
                    ColorName = item.Product.Color.Name,
                    ColorHex = item.Product.Color.Hex,

                    ShapeName = item.Product.Shape.Name,
                    ShapeHasFrame = item.Product.Shape.HasFrame,

                    TransparencyName = item.Product.Transparency.Name,
                    TransparencyDescription = item.Product.Transparency.Description,
                    TransparencyValue = item.Product.Transparency.Value,

                    ProductQuantity = item.ProductQuantity,
                    ProductUnitPrice = item.UnitPrice,
                    EntryTotalValue = (item.ProductQuantity * item.UnitPrice),
                };
                orderDetails.Add(detail);
            }
            await _db.OrderDetails.AddRangeAsync(orderDetails);
            await _db.SaveChangesAsync();
            return Task.CompletedTask;
        }
    }
}
