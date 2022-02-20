using MediatR;
using Microsoft.EntityFrameworkCore;
using Shaper.API.CQRS.ColorData.Commands;
using Shaper.DataAccess.Context;
using Shaper.Models.Entities;

namespace Shaper.API.CQRS.ColorData.Handlers
{
    public class DeleteColorHandler : IRequestHandler<DeleteColorCommand, Color>
    {

        private readonly AppDbContext _db;

        public DeleteColorHandler(AppDbContext db)
        {
            _db = db;
        }

        public async Task<Color> Handle(DeleteColorCommand request, CancellationToken cancellationToken)
        {
            var color = await _db.Colors.FirstOrDefaultAsync(x => x.Id == request.id);
            _db.Remove(color);
            await _db.SaveChangesAsync();
            return color;
        }
    }
}
