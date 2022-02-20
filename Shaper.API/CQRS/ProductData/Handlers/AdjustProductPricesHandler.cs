using MediatR;
using Microsoft.EntityFrameworkCore;
using Shaper.API.CQRS.ProductData.Commands;
using Shaper.DataAccess.Context;
using Shaper.Models.Entities;

namespace Shaper.API.CQRS.ProductData.Handlers
{
    public class AdjustProductPricesHandler : IRequestHandler<AdjustProductPricesCommand, Task>
    {
        private readonly AppDbContext _db;

        public AdjustProductPricesHandler(AppDbContext db)
        {
            _db = db;
        }


        public async Task<Task> Handle(AdjustProductPricesCommand request, CancellationToken cancellationToken)
        {
            var products = await _db.Products.Include(x => x.Color).Include(x => x.Shape).Include(x => x.Transparency).Where(request.Filter).ToListAsync();
            products.ForEach(product => product.Price = (product.Color.AddedValue + product.Shape.AddedValue + product.Transparency.AddedValue));
            _db.UpdateRange(products);
            await _db.SaveChangesAsync();
            return Task.CompletedTask;
        }
    }
}
