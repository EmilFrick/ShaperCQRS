using MediatR;
using Microsoft.EntityFrameworkCore;
using Shaper.API.CQRS.ColorData.Queries;
using Shaper.DataAccess.Context;
using Shaper.Models.Entities;

namespace Shaper.API.CQRS.ColorData.Handlers
{
    public class GetColorsHandler : IRequestHandler<ReadFilteredColorsQuery, List<Color>>
    {
        private readonly AppDbContext _db;


        public GetColorsHandler(AppDbContext db)
        {
            _db = db;
        }

        public async Task<List<Color>> Handle(ReadFilteredColorsQuery request, CancellationToken cancellationToken)
        {
            return await _db.Colors.Include(x=>x.Products).Where(request.filter).ToListAsync();
        }
    }
}
