using MediatR;
using Microsoft.EntityFrameworkCore;
using Shaper.API.CQRS.ColorData.Queries;
using Shaper.DataAccess.Context;
using Shaper.Models.Entities;

namespace Shaper.API.CQRS.ColorData.Handlers
{
    public class ReadColorsHandler : IRequestHandler<ReadColorsQuery, List<Color>>
    {
        private readonly AppDbContext _db;


        public ReadColorsHandler(AppDbContext db)
        {
            _db = db;
        }

        public async Task<List<Color>> Handle(ReadColorsQuery request, CancellationToken cancellationToken)
        {
            return await _db.Colors.Include(x=>x.Products).ToListAsync();
        }
    }
}
