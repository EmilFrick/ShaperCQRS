using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shaper.API.CQRS.ColorData.Commands;
using Shaper.API.CQRS.ColorData.Queries;
using Shaper.API.CQRS.ProductData.Commands;
using Shaper.DataAccess.Context;
using Shaper.DataAccess.Repo;
using Shaper.DataAccess.Repo.IRepo;
using Shaper.Models.Entities;
using Shaper.Models.Models.ColorModels;
using System.Data;
using System.Drawing;
using Color = Shaper.Models.Entities.Color;

namespace Shaper.API.Controllers
{
    [Route("api/[controller]")]
    //[Authorize(Roles = "Admin")]
    [ApiController]
    public class ColorsController : ControllerBase
    {

        private readonly IMediator _mediator;


        public ColorsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetColors()
        {

            var result = await _mediator.Send(new ReadColorsQuery());
            if (result == null)
                return NotFound();

            return Ok(result);
        }

        [AllowAnonymous]
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetColor(int id)
        {
            var result = await _mediator.Send(new ReadColorQuery(x => x.Id == id));
            if (result == null)
                return NotFound();

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> CreateColor(ColorCreateModel color)
        {
            if (ModelState.IsValid)
            {
                var result = await _mediator.Send(new ReadColorQuery(x => x.Name == color.Name || x.Hex == color.Hex));
                if (result is not null)
                    return Conflict(new { message = "The Color youre tying to add already exists. Check 'Color Name' and 'Color Hex'." });

                var addedColor = await _mediator.Send(new CreateColorCommand(color));
                return CreatedAtAction("GetColor", new { id = addedColor.Id }, addedColor);
            }
            else
                return BadRequest();
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateColor(int id, ColorUpdateModel color)
        {
            if (color.Id != id)
                return BadRequest();

            if (ModelState.IsValid)
            {
                var conflictingColor = await _mediator.Send(new ReadColorQuery(x => x.Id != color.Id && x.Hex == color.Hex || x.Id != color.Id && x.Name == color.Name));
                
                if (conflictingColor is not null)
                    return Conflict(new ColorUpdateModel(conflictingColor));

                var updatedColor = await _mediator.Send(new UpdateColorCommand(color));

                if (updatedColor.Products.Count > 0)
                    await _mediator.Send(new AdjustProductPricesCommand(x => x.Color.Id == updatedColor.Id));
                
                return Ok();
            }
            else
                return BadRequest();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteColor(int id)
        {
            var color = await _mediator.Send(new ReadColorQuery(x => x.Id == id));
            if (color is not null || color.Name is not "Default")
            {
                if (color.Products?.Count > 0)
                    await _mediator.Send(new RemovedColorUpdateProductsCommand(id));

                var deletedColor = await _mediator.Send(new DeleteColorCommand(id));
                return Ok();
            }
            else
                return BadRequest();
        }
    }
}
