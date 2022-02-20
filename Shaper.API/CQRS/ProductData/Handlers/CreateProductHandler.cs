using MediatR;
using Shaper.API.CQRS.ProductData.Commands;
using Shaper.DataAccess.Context;
using Shaper.Models.Entities;

namespace Shaper.API.CQRS.ProductData.Handlers
{
    public class CreateProductHandler : IRequestHandler<CreateProductCommand, Product>
    {

        private readonly AppDbContext _db;

        public CreateProductHandler(AppDbContext db)
        {
            _db = db;
        }

        public async Task<Product> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            var color = new Product(request.ProductCreateModel);
            await _db.AddAsync(color);
            await _db.SaveChangesAsync();
            return color;
        }
    }
}
