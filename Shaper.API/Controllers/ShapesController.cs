using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shaper.API.CQRS.ShapeData.Commands;
using Shaper.API.CQRS.ShapeData.Queries;
using Shaper.API.CQRS.ProductData.Commands;
using Shaper.Models.Entities;
using Shaper.Models.Models.ShapeModels;
using Shaper.Models.Models.ShapeModels;
using System.Data;
using System.Drawing;

namespace Shaper.API.Controllers
{
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    [ApiController]
    public class ShapesController : ControllerBase
    {

        private readonly IMediator _mediator;

        public ShapesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetShapes()
        {

            var result = await _mediator.Send(new ReadShapesQuery());
            if (result == null)
                return NotFound();

            return Ok(result);
        }

        [AllowAnonymous]
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetShape(int id)
        {
            var result = await _mediator.Send(new ReadShapeQuery(x => x.Id == id));
            if (result == null)
                return NotFound();

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> CreateShape(ShapeCreateModel shape)
        {
            if (ModelState.IsValid)
            {
                var result = await _mediator.Send(new ReadShapeQuery(x => x.Name == shape.Name && x.HasFrame == shape.HasFrame));
                if (result is not null)
                    return Conflict(new { message = "The Shape youre tying to add already exists. Check 'Shape Name' and 'Shape Frame'." });

                var addedShape = await _mediator.Send(new CreateShapeCommand(shape));
                return CreatedAtAction("GetShape", new { id = addedShape.Id }, addedShape);
            }
            else
                return BadRequest();
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateShape(int id, ShapeUpdateModel shape)
        {
            if (shape.Id != id)
                return BadRequest();

            if (ModelState.IsValid)
            {
                var conflictingShape = await _mediator.Send(new ReadShapeQuery(x =>  x.Id != shape.Id && x.Name == shape.Name && x.HasFrame == shape.HasFrame));
                
                if (conflictingShape is not null)
                    return Conflict(new ShapeUpdateModel(conflictingShape));

                var updatedShape = await _mediator.Send(new UpdateShapeCommand(shape));

                if (updatedShape.Products.Count > 0)
                    await _mediator.Send(new AdjustProductPricesCommand(x => x.Shape.Id == updatedShape.Id));
                
                return Ok();
            }
            else
                return BadRequest("Something went wrong.");
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteShape(int id)
        {
            var shape = await _mediator.Send(new ReadShapeQuery(x => x.Id == id));
            if (shape is not null || shape.Name is not "Default")
            {
                if (shape.Products?.Count > 0)
                    await _mediator.Send(new RemovedShapeUpdateProductsCommand(id));

                var deletedShape = await _mediator.Send(new DeleteShapeCommand(id));
                return Ok();
            }
            else
                return BadRequest();
        }
    }
}
