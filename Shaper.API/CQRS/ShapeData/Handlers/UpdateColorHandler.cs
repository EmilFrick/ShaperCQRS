using MediatR;
using Shaper.API.CQRS.ShapeData.Commands;
using Shaper.API.CQRS.ShapeData.Queries;
using Shaper.DataAccess.Context;
using Shape = Shaper.Models.Entities.Shape;


namespace Shaper.API.CQRS.ShapeData.Handlers
{
    public class UpdateShapeHandler : IRequestHandler<UpdateShapeCommand, Shape>
    {
        private readonly AppDbContext _db;
        private readonly IMediator _mediator;



        public UpdateShapeHandler(AppDbContext db, IMediator mediator)
        {
            _db = db;
            _mediator = mediator;
        }

        public async Task<Shape> Handle(UpdateShapeCommand request, CancellationToken cancellationToken)
        {
            var originalShape = await _mediator.Send(new ReadShapeQuery(x=>x.Id == request.Model.Id));
            originalShape.Name = request.Model.Name;
            originalShape.HasFrame = request.Model.HasFrame;
            originalShape.AddedValue = request.Model.AddedValue;
            _db.Shapes.Update(originalShape);
            await _db.SaveChangesAsync();
            return originalShape;
        }
    }
}
