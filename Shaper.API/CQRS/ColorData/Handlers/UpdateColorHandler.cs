using MediatR;
using Shaper.API.CQRS.ColorData.Commands;
using Shaper.API.CQRS.ColorData.Queries;
using Shaper.DataAccess.Context;
using Color = Shaper.Models.Entities.Color;


namespace Shaper.API.CQRS.ColorData.Handlers
{
    public class UpdateColorHandler : IRequestHandler<UpdateColorCommand, Color>
    {
        private readonly AppDbContext _db;
        private readonly IMediator _mediator;



        public UpdateColorHandler(AppDbContext db, IMediator mediator)
        {
            _db = db;
            _mediator = mediator;
        }

        public async Task<Color> Handle(UpdateColorCommand request, CancellationToken cancellationToken)
        {
            var originalColor = await _mediator.Send(new ReadColorQuery(x=>x.Id == request.Model.Id));
            originalColor.Name = request.Model.Name;
            originalColor.Hex = request.Model.Hex;
            originalColor.AddedValue = request.Model.AddedValue;
            _db.Colors.Update(originalColor);
            await _db.SaveChangesAsync();
            return originalColor;
        }
    }
}
