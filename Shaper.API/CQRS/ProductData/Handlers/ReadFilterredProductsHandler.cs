using MediatR;
using Microsoft.EntityFrameworkCore;
using Shaper.API.CQRS.ProductData.Queries;
using Shaper.DataAccess.Context;
using Shaper.Models.Entities;

namespace Shaper.API.CQRS.ProductData.Handlers
{
    public class GetProductsHandler : IRequestHandler<ReadFilteredProductsQuery, List<Product>>
    {
        private readonly AppDbContext _db;


        public GetProductsHandler(AppDbContext db)
        {
            _db = db;
        }

        public async Task<List<Product>> Handle(ReadFilteredProductsQuery request, CancellationToken cancellationToken)
        {
            return await _db.Products.Include(x=>x.Color).Include(x=>x.Shape).Include(x=>x.Transparency).Where(request.filter).ToListAsync();
        }
    }
}
