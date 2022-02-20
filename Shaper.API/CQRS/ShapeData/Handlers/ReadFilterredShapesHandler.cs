using MediatR;
using Microsoft.EntityFrameworkCore;
using Shaper.API.CQRS.ShapeData.Queries;
using Shaper.DataAccess.Context;
using Shaper.Models.Entities;

namespace Shaper.API.CQRS.ShapeData.Handlers
{
    public class GetShapesHandler : IRequestHandler<ReadFilteredShapesQuery, List<Shape>>
    {
        private readonly AppDbContext _db;


        public GetShapesHandler(AppDbContext db)
        {
            _db = db;
        }

        public async Task<List<Shape>> Handle(ReadFilteredShapesQuery request, CancellationToken cancellationToken)
        {
            return await _db.Shapes.Include(x=>x.Products).Where(request.filter).ToListAsync();
        }
    }
}
