using MediatR;
using Microsoft.EntityFrameworkCore;
using Shaper.API.CQRS.ShapeData.Queries;
using Shaper.DataAccess.Context;
using Shaper.Models.Entities;

namespace Shaper.API.CQRS.ShapeData.Handlers
{
    public class ReadShapesHandler : IRequestHandler<ReadShapesQuery, List<Shape>>
    {
        private readonly AppDbContext _db;


        public ReadShapesHandler(AppDbContext db)
        {
            _db = db;
        }

        public async Task<List<Shape>> Handle(ReadShapesQuery request, CancellationToken cancellationToken)
        {
            return await _db.Shapes.Include(x=>x.Products).ToListAsync();
        }
    }
}
