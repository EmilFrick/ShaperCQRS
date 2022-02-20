using MediatR;
using Microsoft.EntityFrameworkCore;
using Shaper.API.CQRS.ShapeData.Commands;
using Shaper.DataAccess.Context;
using Shaper.Models.Entities;

namespace Shaper.API.CQRS.ShapeData.Handlers
{
    public class DeleteShapeHandler : IRequestHandler<DeleteShapeCommand, Shape>
    {

        private readonly AppDbContext _db;

        public DeleteShapeHandler(AppDbContext db)
        {
            _db = db;
        }

        public async Task<Shape> Handle(DeleteShapeCommand request, CancellationToken cancellationToken)
        {
            var shape = await _db.Shapes.FirstOrDefaultAsync(x => x.Id == request.Id);
            _db.Remove(shape);
            await _db.SaveChangesAsync();
            return shape;
        }
    }
}
