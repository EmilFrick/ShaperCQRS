using MediatR;
using Microsoft.EntityFrameworkCore;
using Shaper.API.CQRS.TransparencyData.Commands;
using Shaper.DataAccess.Context;
using Shaper.Models.Entities;

namespace Shaper.API.CQRS.TransparencyData.Handlers
{
    public class DeleteTransparencyHandler : IRequestHandler<DeleteTransparencyCommand, Transparency>
    {

        private readonly AppDbContext _db;

        public DeleteTransparencyHandler(AppDbContext db)
        {
            _db = db;
        }

        public async Task<Transparency> Handle(DeleteTransparencyCommand request, CancellationToken cancellationToken)
        {
            var transparency = await _db.Transparencies.FirstOrDefaultAsync(x => x.Id == request.Id);
            _db.Remove(transparency);
            await _db.SaveChangesAsync();
            return transparency;
        }
    }
}
