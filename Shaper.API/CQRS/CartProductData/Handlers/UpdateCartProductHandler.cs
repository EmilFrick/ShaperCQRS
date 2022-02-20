using MediatR;
using Shaper.API.CQRS.CartProductData.Commands;
using Shaper.API.CQRS.CartProductData.Queries;
using Shaper.DataAccess.Context;
using Shaper.Models.Entities;
using CartProduct = Shaper.Models.Entities.CartProduct;


namespace Shaper.API.CQRS.CartProductData.Handlers
{
    public class UpdateCartProductHandler : IRequestHandler<UpdateCartProductCommand, CartProduct>
    {
        private readonly AppDbContext _db;
        private readonly IMediator _mediator;



        public UpdateCartProductHandler(AppDbContext db, IMediator mediator)
        {
            _db = db;
            _mediator = mediator;
        }

        public async Task<CartProduct> Handle(UpdateCartProductCommand request, CancellationToken cancellationToken)
        {
            request.product.ProductQuantity = request.quantityUpdate;
            _db.CartProducts.Update(request.product);
            await _db.SaveChangesAsync();
            return request.product;
        }
    }
}
