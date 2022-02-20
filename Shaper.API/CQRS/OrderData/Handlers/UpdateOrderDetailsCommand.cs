using MediatR;
using Shaper.API.CQRS.OrderData.Commands;
using Shaper.API.CQRS.OrderData.Queries;
using Shaper.DataAccess.Context;
using Shaper.Models.Entities;
using Order = Shaper.Models.Entities.Order;


namespace Shaper.API.CQRS.OrderData.Handlers
{
    public class UpdateOrderDetailsHandler : IRequestHandler<UpdateOrderDetailsCommand, List<OrderDetail>>
    {
        private readonly AppDbContext _db;
        private readonly IMediator _mediator;



        public UpdateOrderDetailsHandler(AppDbContext db, IMediator mediator)
        {
            _db = db;
            _mediator = mediator;
        }

        public async Task<List<OrderDetail>> Handle(UpdateOrderDetailsCommand request, CancellationToken cancellationToken)
        {
            foreach (var updatedOrderEntry in request.updatedOrderEntries)
            {
                foreach (var orderDetail in request.originalOrderEntries)
                {
                    if (updatedOrderEntry.Id == orderDetail.Id)
                    {

                        if (updatedOrderEntry?.ProductId is not null)
                            orderDetail.ProductId = updatedOrderEntry.ProductId.GetValueOrDefault();

                        if (updatedOrderEntry?.ProductName is not null)
                            orderDetail.ProductName = updatedOrderEntry.ProductName;

                        if (updatedOrderEntry?.ColorName is not null)
                            orderDetail.ColorName = updatedOrderEntry.ColorName;

                        if (updatedOrderEntry?.ColorHex is not null)
                            orderDetail.ColorHex = updatedOrderEntry.ColorHex;

                        if (updatedOrderEntry?.ShapeName is not null)
                            orderDetail.ShapeName = updatedOrderEntry.ShapeName;

                        if (updatedOrderEntry?.ShapeHasFrame is not null)
                            orderDetail.ShapeHasFrame = updatedOrderEntry.ShapeHasFrame.GetValueOrDefault();

                        if (updatedOrderEntry?.TransparencyName is not null)
                            orderDetail.TransparencyName = updatedOrderEntry.TransparencyName;

                        if (updatedOrderEntry?.TransparencyDescription is not null)
                            orderDetail.TransparencyDescription = updatedOrderEntry.TransparencyDescription;

                        if (updatedOrderEntry?.TransparencyValue is not null)
                            orderDetail.TransparencyValue = updatedOrderEntry.TransparencyValue.GetValueOrDefault();

                        if (updatedOrderEntry?.ProductQuantity is not null)
                            orderDetail.ProductQuantity = updatedOrderEntry.ProductQuantity.GetValueOrDefault();

                        if (updatedOrderEntry?.ProductUnitPrice is not null)
                            orderDetail.ProductUnitPrice = updatedOrderEntry.ProductUnitPrice.GetValueOrDefault();

                        if (updatedOrderEntry?.ProductUnitPrice is not null || updatedOrderEntry?.ProductQuantity is not null)
                            orderDetail.EntryTotalValue = (orderDetail.ProductUnitPrice * orderDetail.ProductQuantity);
                    }
                }
            }

            return request.originalOrderEntries;
        }
    }
}
