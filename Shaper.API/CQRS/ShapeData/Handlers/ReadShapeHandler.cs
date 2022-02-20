using MediatR;
using Microsoft.EntityFrameworkCore;
using Shaper.API.CQRS.ShapeData.Queries;
using Shaper.DataAccess.Context;
using Shaper.Models.Entities;

namespace Shaper.API.CQRS.ShapeData.Handlers
{
    public class ReadShapeHandler : IRequestHandler<ReadShapeQuery, Shape>
    {
        private readonly AppDbContext _db;

        public ReadShapeHandler(AppDbContext db)
        {
            _db = db;
        }

        public async Task<Shape> Handle(ReadShapeQuery request, CancellationToken cancellationToken)
        {
            return await _db.Shapes.Include(x => x.Products).FirstOrDefaultAsync(request.filter);
        }
    }
}
