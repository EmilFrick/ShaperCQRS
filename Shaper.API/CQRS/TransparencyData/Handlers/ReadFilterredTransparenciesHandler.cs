using MediatR;
using Microsoft.EntityFrameworkCore;
using Shaper.API.CQRS.TransparencyData.Queries;
using Shaper.DataAccess.Context;
using Shaper.Models.Entities;

namespace Shaper.API.CQRS.TransparencyData.Handlers
{
    public class GetTransparencysHandler : IRequestHandler<ReadFilteredTransparenciesQuery, List<Transparency>>
    {
        private readonly AppDbContext _db;


        public GetTransparencysHandler(AppDbContext db)
        {
            _db = db;
        }

        public async Task<List<Transparency>> Handle(ReadFilteredTransparenciesQuery request, CancellationToken cancellationToken)
        {
            return await _db.Transparencies.Include(x=>x.Products).Where(request.filter).ToListAsync();
        }
    }
}
