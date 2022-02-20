using MediatR;
using Microsoft.EntityFrameworkCore;
using Shaper.API.CQRS.CartProductData.Queries;
using Shaper.DataAccess.Context;
using Shaper.Models.Entities;

namespace Shaper.API.CQRS.CartProductData.Handlers
{
    public class ReadCartProductHandler : IRequestHandler<ReadCartProductQuery, CartProduct>
    {
        private readonly AppDbContext _db;

        public ReadCartProductHandler(AppDbContext db)
        {
            _db = db;
        }

        public async Task<CartProduct> Handle(ReadCartProductQuery request, CancellationToken cancellationToken)
        {
            return await _db.CartProducts.Include(x => x.Product).FirstOrDefaultAsync(request.filter);
        }
    }
}
