using MediatR;
using Microsoft.EntityFrameworkCore;
using Shaper.API.CQRS.ColorData.Queries;
using Shaper.DataAccess.Context;
using Shaper.Models.Entities;

namespace Shaper.API.CQRS.ColorData.Handlers
{
    public class ReadDefaultColorHandler : IRequestHandler<ReadDefaultColorQuery, Color>
    {
        private readonly AppDbContext _db;


        public ReadDefaultColorHandler(AppDbContext db)
        {
            _db = db;
        }

        public async Task<Color> Handle(ReadDefaultColorQuery request, CancellationToken cancellationToken)
        {
            var defaultColor = await _db.Colors.FirstOrDefaultAsync(x => x.Name == "Default");
            if (defaultColor is null)
            {
                defaultColor = new Color
                {
                    Name = "Default",
                    Hex = "#000000",
                    AddedValue = 0,
                };
                await _db.Colors.AddAsync(defaultColor);
                await _db.SaveChangesAsync();
            }
            return defaultColor;
        }
    }
}
