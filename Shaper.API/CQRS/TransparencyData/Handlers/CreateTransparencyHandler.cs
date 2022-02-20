using MediatR;
using Shaper.API.CQRS.TransparencyData.Commands;
using Shaper.DataAccess.Context;
using Shaper.Models.Entities;

namespace Shaper.API.CQRS.TransparencyData.Handlers
{
    public class CreateTransparencyHandler : IRequestHandler<CreateTransparencyCommand, Transparency>
    {

        private readonly AppDbContext _db;

        public CreateTransparencyHandler(AppDbContext db)
        {
            _db = db;
        }

        public async Task<Transparency> Handle(CreateTransparencyCommand request, CancellationToken cancellationToken)
        {
            var transparency = new Transparency(request.TransparencyCreateModel);
            await _db.AddAsync(transparency);
            await _db.SaveChangesAsync();
            return transparency;
        }
    }
}
