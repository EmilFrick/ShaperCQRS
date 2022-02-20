using MediatR;
using Microsoft.EntityFrameworkCore;
using Shaper.API.CQRS.TransparencyData.Queries;
using Shaper.DataAccess.Context;
using Shaper.Models.Entities;

namespace Shaper.API.CQRS.TransparencyData.Handlers
{
    public class ReadTransparencyHandler : IRequestHandler<ReadTransparencyQuery, Transparency>
    {
        private readonly AppDbContext _db;

        public ReadTransparencyHandler(AppDbContext db)
        {
            _db = db;
        }

        public async Task<Transparency> Handle(ReadTransparencyQuery request, CancellationToken cancellationToken)
        {
            return await _db.Transparencies.Include(x => x.Products).FirstOrDefaultAsync(request.filter);
        }
    }
}
