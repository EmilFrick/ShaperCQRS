using MediatR;
using Microsoft.EntityFrameworkCore;
using Shaper.API.CQRS.ProductData.Queries;
using Shaper.DataAccess.Context;
using Shaper.Models.Entities;

namespace Shaper.API.CQRS.ProductData.Handlers
{
    public class ReadProductHandler : IRequestHandler<ReadProductQuery, Product>
    {
        private readonly AppDbContext _db;

        public ReadProductHandler(AppDbContext db)
        {
            _db = db;
        }

        public async Task<Product> Handle(ReadProductQuery request, CancellationToken cancellationToken)
        {
            return await _db.Products.Include(x=>x.Color).Include(x=>x.Shape).Include(x=>x.Transparency).FirstOrDefaultAsync(request.filter);
        }
    }
}
