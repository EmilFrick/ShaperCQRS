using MediatR;
using Shaper.API.CQRS.ShapeData.Commands;
using Shaper.DataAccess.Context;
using Shaper.Models.Entities;

namespace Shaper.API.CQRS.ShapeData.Handlers
{
    public class CreateShapeHandler : IRequestHandler<CreateShapeCommand, Shape>
    {

        private readonly AppDbContext _db;

        public CreateShapeHandler(AppDbContext db)
        {
            _db = db;
        }

        public async Task<Shape> Handle(CreateShapeCommand request, CancellationToken cancellationToken)
        {
            var shape = new Shape(request.ShapeCreateModel);
            await _db.AddAsync(shape);
            await _db.SaveChangesAsync();
            return shape;
        }
    }
}
