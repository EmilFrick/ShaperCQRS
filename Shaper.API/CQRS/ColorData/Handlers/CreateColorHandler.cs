using MediatR;
using Shaper.API.CQRS.ColorData.Commands;
using Shaper.DataAccess.Context;
using Shaper.Models.Entities;

namespace Shaper.API.CQRS.ColorData.Handlers
{
    public class CreateColorHandler : IRequestHandler<CreateColorCommand, Color>
    {

        private readonly AppDbContext _db;

        public CreateColorHandler(AppDbContext db)
        {
            _db = db;
        }

        public async Task<Color> Handle(CreateColorCommand request, CancellationToken cancellationToken)
        {
            var color = new Color(request.ColorCreateModel);
            await _db.AddAsync(color);
            await _db.SaveChangesAsync();
            return color;
        }
    }
}
