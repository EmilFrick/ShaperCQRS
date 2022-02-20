using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shaper.API.CQRS.TransparencyData.Commands;
using Shaper.API.CQRS.TransparencyData.Queries;
using Shaper.API.CQRS.ProductData.Commands;
using Shaper.DataAccess.Repo.IRepo;
using Shaper.Models.Entities;
using Shaper.Models.Models.TransparencyModels;
using Shaper.Models.Models.TransparencyModels;
using System.Data;

namespace Shaper.API.Controllers
{
    [Route("api/[controller]")]
    //[Authorize(Roles = "Admin")]
    [ApiController]
    public class TransparenciesController : ControllerBase
    {
        private readonly IUnitOfWork _db;
        private readonly IMediator _mediator;


        public TransparenciesController(IUnitOfWork db, IMediator mediator)
        {
            _db = db;
            _mediator = mediator;
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetTransparencies()
        {

            var result = await _mediator.Send(new ReadTransparenciesQuery());
            if (result == null)
                return NotFound();

            return Ok(result);
        }

        [AllowAnonymous]
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetTransparency(int id)
        {
            var result = await _mediator.Send(new ReadTransparencyQuery(x => x.Id == id));
            if (result == null)
                return NotFound();

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> CreateTransparency(TransparencyCreateModel transparency)
        {
            if (ModelState.IsValid)
            {
                var result = await _mediator.Send(new ReadTransparencyQuery(x => x.Name == transparency.Name || x.Value == transparency.Value));
                if (result is not null)
                    return Conflict(new { message = "The Transparency youre tying to add already exists. Check 'Transparency Name' and 'Transparency Hex'." });

                var addedTransparency = await _mediator.Send(new CreateTransparencyCommand(transparency));
                return CreatedAtAction("GetTransparency", new { id = addedTransparency.Id }, addedTransparency);
            }
            else
                return BadRequest();
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateTransparency(int id, TransparencyUpdateModel transparency)
        {
            if (transparency.Id != id)
                return BadRequest();

            if (ModelState.IsValid)
            {
                var conflictingTransparency = await _mediator.Send(new ReadTransparencyQuery(x => x.Id != transparency.Id && x.Value == transparency.Value || x.Id != transparency.Id && x.Name == transparency.Name));
                
                if (conflictingTransparency is not null)
                    return Conflict(new TransparencyUpdateModel(conflictingTransparency));

                var updatedTransparency = await _mediator.Send(new UpdateTransparencyCommand(transparency));

                if (updatedTransparency.Products.Count > 0)
                    await _mediator.Send(new AdjustProductPricesCommand(x => x.Transparency.Id == updatedTransparency.Id));
                
                return Ok();
            }
            else
                return BadRequest();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteTransparency(int id)
        {
            var transparency = await _mediator.Send(new ReadTransparencyQuery(x => x.Id == id));
            if (transparency is not null || transparency.Name is not "Default")
            {
                if (transparency.Products?.Count > 0)
                    await _mediator.Send(new RemovedTransparencyUpdateProductsCommand(id));

                var deletedTransparency = await _mediator.Send(new DeleteTransparencyCommand(id));
                return Ok();
            }
            else
                return BadRequest();
        }
    }
}
