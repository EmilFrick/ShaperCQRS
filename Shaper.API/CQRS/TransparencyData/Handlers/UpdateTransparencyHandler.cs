using MediatR;
using Shaper.API.CQRS.TransparencyData.Commands;
using Shaper.API.CQRS.TransparencyData.Queries;
using Shaper.DataAccess.Context;
using Transparency = Shaper.Models.Entities.Transparency;


namespace Shaper.API.CQRS.TransparencyData.Handlers
{
    public class UpdateTransparencyHandler : IRequestHandler<UpdateTransparencyCommand, Transparency>
    {
        private readonly AppDbContext _db;
        private readonly IMediator _mediator;



        public UpdateTransparencyHandler(AppDbContext db, IMediator mediator)
        {
            _db = db;
            _mediator = mediator;
        }

        public async Task<Transparency> Handle(UpdateTransparencyCommand request, CancellationToken cancellationToken)
        {
            var originalTransparency = await _mediator.Send(new ReadTransparencyQuery(x=>x.Id == request.Model.Id));
            originalTransparency.Name = request.Model.Name;
            originalTransparency.Description = request.Model.Description;
            originalTransparency.Value = request.Model.Value;
            originalTransparency.AddedValue = request.Model.AddedValue;
            _db.Transparencies.Update(originalTransparency);
            await _db.SaveChangesAsync();
            return originalTransparency;
        }
    }
}
