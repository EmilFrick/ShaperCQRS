using MediatR;
using Microsoft.EntityFrameworkCore;
using Shaper.API.CQRS.ProductData.Commands;
using Shaper.DataAccess.Context;
using Shaper.Models.Entities;

namespace Shaper.API.CQRS.ProductData.Handlers
{
    public class DeleteProductHandler : IRequestHandler<DeleteProductCommand, Product>
    {

        private readonly AppDbContext _db;

        public DeleteProductHandler(AppDbContext db)
        {
            _db = db;
        }

        public async Task<Product> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
        {
            var color = await _db.Products.FirstOrDefaultAsync(x => x.Id == request.id);
            _db.Remove(color);
            await _db.SaveChangesAsync();
            return color;
        }
    }
}
