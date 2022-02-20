using MediatR;
using Microsoft.EntityFrameworkCore;
using Shaper.API.CQRS.TransparencyData.Queries;
using Shaper.DataAccess.Context;
using Shaper.Models.Entities;

namespace Shaper.API.CQRS.TransparencyData.Handlers
{
    public class ReadDefaultTransparencyHandler : IRequestHandler<ReadDefaultTransparencyQuery, Transparency>
    {
        private readonly AppDbContext _db;


        public ReadDefaultTransparencyHandler(AppDbContext db)
        {
            _db = db;
        }

        public async Task<Transparency> Handle(ReadDefaultTransparencyQuery request, CancellationToken cancellationToken)
        {
            var defaultTransparency = await _db.Transparencies.FirstOrDefaultAsync(x => x.Name == "Default");
            if (defaultTransparency is null)
            {
                defaultTransparency = new Transparency
                {
                    Name = "Default",
                    Value = 0,
                    AddedValue = 0,
                };
                await _db.Transparencies.AddAsync(defaultTransparency);
                await _db.SaveChangesAsync();
            }
            return defaultTransparency;
        }
    }
}
