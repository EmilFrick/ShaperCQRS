using MediatR;
using Shaper.API.CQRS.ProductData.Commands;
using Shaper.API.CQRS.ProductData.Queries;
using Shaper.DataAccess.Context;
using Product = Shaper.Models.Entities.Product;


namespace Shaper.API.CQRS.ProductData.Handlers
{
    public class UpdateProductHandler : IRequestHandler<UpdateProductCommand, Product>
    {
        private readonly AppDbContext _db;
        private readonly IMediator _mediator;



        public UpdateProductHandler(AppDbContext db, IMediator mediator)
        {
            _db = db;
            _mediator = mediator;
        }

        public async Task<Product> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
        {
            var originalProduct = await _mediator.Send(new ReadProductQuery(x=>x.Id == request.Model.Id));
            originalProduct.Name = request.Model.Name;
            originalProduct.Description = request.Model.Description;
            originalProduct.Artist = request.Model.Artist;
            originalProduct.ColorId = request.Model.ColorId;
            originalProduct.ShapeId = request.Model.ShapeId;
            originalProduct.TransparencyId = request.Model.TransparencyId;

            _db.Products.Update(originalProduct);
            await _db.SaveChangesAsync();
            return originalProduct;
        }
    }
}
