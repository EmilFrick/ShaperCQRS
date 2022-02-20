using MediatR;
using Microsoft.EntityFrameworkCore;
using Shaper.API.CQRS.ColorData.Queries;
using Shaper.DataAccess.Context;
using Shaper.Models.Entities;

namespace Shaper.API.CQRS.ColorData.Handlers
{
    public class ReadColorHandler : IRequestHandler<ReadColorQuery, Color>
    {
        private readonly AppDbContext _db;

        public ReadColorHandler(AppDbContext db)
        {
            _db = db;
        }

        public async Task<Color> Handle(ReadColorQuery request, CancellationToken cancellationToken)
        {
            return await _db.Colors.Include(x => x.Products).FirstOrDefaultAsync(request.filter);
        }
    }
}
