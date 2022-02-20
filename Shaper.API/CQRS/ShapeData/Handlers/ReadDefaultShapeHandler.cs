using MediatR;
using Microsoft.EntityFrameworkCore;
using Shaper.API.CQRS.ShapeData.Queries;
using Shaper.DataAccess.Context;
using Shaper.Models.Entities;

namespace Shaper.API.CQRS.ShapeData.Handlers
{
    public class ReadDefaultShapeHandler : IRequestHandler<ReadDefaultShapeQuery, Shape>
    {
        private readonly AppDbContext _db;


        public ReadDefaultShapeHandler(AppDbContext db)
        {
            _db = db;
        }

        public async Task<Shape> Handle(ReadDefaultShapeQuery request, CancellationToken cancellationToken)
        {
            var defaultShape = await _db.Shapes.FirstOrDefaultAsync(x => x.Name == "Default");
            if (defaultShape is null)
            {
                defaultShape = new Shape
                {
                    Name = "Default",
                    HasFrame = false,
                    AddedValue = 0,
                };

                await _db.Shapes.AddAsync(defaultShape);
                await _db.SaveChangesAsync();
            }
            return defaultShape;
        }
    }
}
