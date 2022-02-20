using MediatR;
using Shaper.API.CQRS.CartProductData.Commands;
using Shaper.DataAccess.Context;
using Shaper.Models.Entities;

namespace Shaper.API.CQRS.CartProductData.Handlers
{
    public class CreateCartProductHandler : IRequestHandler<CreateCartProductCommand, CartProduct>
    {

        private readonly AppDbContext _db;

        public CreateCartProductHandler(AppDbContext db)
        {
            _db = db;
        }

        public async Task<CartProduct> Handle(CreateCartProductCommand request, CancellationToken cancellationToken)
        {
            var color = new CartProduct();
            await _db.AddAsync(color);
            await _db.SaveChangesAsync();
            return color;
        }
    }
}
