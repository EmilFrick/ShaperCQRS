using MediatR;
using Microsoft.EntityFrameworkCore;
using Shaper.API.CQRS.CartProductData.Queries;
using Shaper.DataAccess.Context;
using Shaper.Models.Entities;

namespace Shaper.API.CQRS.CartProductData.Handlers
{
    public class GetCartProductsHandler : IRequestHandler<ReadFilteredCartProductsQuery, List<CartProduct>>
    {
        private readonly AppDbContext _db;


        public GetCartProductsHandler(AppDbContext db)
        {
            _db = db;
        }

        public async Task<List<CartProduct>> Handle(ReadFilteredCartProductsQuery request, CancellationToken cancellationToken)
        {
            return await _db.CartProducts.Include(x=>x.Product).Where(request.filter).ToListAsync();
        }
    }
}
