using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shaper.API.CQRS.ColorData.Queries;
using Shaper.API.CQRS.ProductData.Commands;
using Shaper.API.CQRS.ProductData.Handlers;
using Shaper.API.CQRS.ProductData.Queries;
using Shaper.API.CQRS.ShapeData.Queries;
using Shaper.API.CQRS.TransparencyData.Queries;
using Shaper.Models.Entities;
using Shaper.Models.Models.ProductComponentsModels;
using Shaper.Models.Models.ProductModels;

namespace Shaper.API.Controllers
{

    [Route("api/[controller]")]
    //[Authorize(Roles = "Artist")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ProductsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [AllowAnonymous]
        [HttpGet("UpsertVM")]
        public async Task<IActionResult> GetProductVMs()
        {
            var colors = await _mediator.Send(new ReadColorsQuery());
            var shapes = await _mediator.Send(new ReadShapesQuery());
            var transparencies = await _mediator.Send(new ReadTransparenciesQuery());
            ProductUpsertModel productVM = new(colors, shapes, transparencies);

            if (productVM.Colors is null || productVM.Shapes is null || productVM.Transparencies is null)
                return Conflict();

            return Ok(productVM);
        }

        [AllowAnonymous]
        [HttpGet("UpsertVM/{id:int}")]
        public async Task<IActionResult> GetProductVMs(int id)
        {
            var product = await _mediator.Send(new ReadProductQuery(x => x.Id == id));
            if (product == null)
                return NotFound();

            var colors = await _mediator.Send(new ReadColorsQuery());
            var shapes = await _mediator.Send(new ReadShapesQuery());
            var transparencies = await _mediator.Send(new ReadTransparenciesQuery());
            ProductUpsertModel productVM = new(product, colors, shapes, transparencies);

            if (productVM.Colors is null || productVM.Shapes is null || productVM.Transparencies is null)
                return Conflict();

            return Ok(productVM);
        }

        [AllowAnonymous]
        [HttpGet("ProductComponents")]
        public async Task<IActionResult> GetProductComponents(ProductReqComponentsModel request)
        {
            var res = await _mediator.Send(new ReadProductComponentsQuery(request));
            if (res.ColorComponent is null || res.ShapeComponent is null || res.TransparencyComponent is null)
                return NotFound();

            return Ok(res);
        }


        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetProducts()
        {
            var result = await _mediator.Send(new ReadProductsQuery());
            if (result is null)
                return NotFound();

            return Ok(result);
        }

        [AllowAnonymous]
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetProduct(int id)
        {
            var result = await _mediator.Send(new ReadProductQuery(p => p.Id == id));
            if (result is null)
                return NotFound();

            return Ok(result);
        }

        [AllowAnonymous]
        [HttpGet("Colors/{id:int}")]
        public async Task<IActionResult> GetProductByColorId(int id)
        {
            var result = await _mediator.Send(new ReadProductsByColorIdQuery(id));
            if (result is null)
                return NotFound();

            return Ok(result);
        }
        [AllowAnonymous]
        [HttpGet("Shapes/{id:int}")]
        public async Task<IActionResult> GetProductByShapeId(int id)
        {
            var result = await _mediator.Send(new ReadProductsByShapeIdQuery(id));
            if (result is null)
                return NotFound();

            return Ok(result);
        }
        [AllowAnonymous]
        [HttpGet("Transparencies/{id:int}")]
        public async Task<IActionResult> GetProductByTransparencyId(int id)
        {
            var result = await _mediator.Send(new ReadProductsByTransparencyIdQuery(id));
            if (result is null)
                return NotFound();

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> CreateProduct(ProductCreateModel product)
        {
            var componentsExist = await _mediator.Send(new ReadProductComponentsQuery(new ProductReqComponentsModel(product.ColorId, product.ShapeId, product.TransparencyId)));
            if (componentsExist.ColorComponent is null || componentsExist.ShapeComponent is null || componentsExist.TransparencyComponent is null)
                return Conflict(product);

            if (ModelState.IsValid)
            {
                var result = await _mediator.Send(new ReadProductQuery(x => x.Name == product.Name || x.ColorId == product.ColorId && x.ShapeId == product.ShapeId && x.TransparencyId == product.TransparencyId));
                if (result is not null)
                    return Conflict(result);

                var createdProduct = await _mediator.Send(new CreateProductCommand(product));
                await _mediator.Send(new AdjustProductPricesCommand(x => x.Id == createdProduct.Id));
                return Ok(createdProduct);
            }
            else
                return BadRequest();
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateProduct(int id, ProductUpdateModel product)
        {
            if (product.Id != id)
                return BadRequest();

            var componentsExist = await _mediator.Send(new ReadProductComponentsQuery(new ProductReqComponentsModel(product.ColorId, product.ShapeId, product.TransparencyId)));
            if (componentsExist.ColorComponent is null || componentsExist.ShapeComponent is null || componentsExist.TransparencyComponent is null)
                return Conflict(product);

            if (ModelState.IsValid)
            {
                var result = await _mediator.Send(new ReadProductQuery(x => x.Id != product.Id & x.Name == product.Name || x.Id != product.Id && x.ColorId == product.ColorId && x.ShapeId == product.ShapeId && x.TransparencyId == product.TransparencyId));

                if (result is not null)
                    return Conflict(result);

                var updatedProduct = await _mediator.Send(new UpdateProductCommand(product));
                await _mediator.Send(new AdjustProductPricesCommand(x => x.Id == updatedProduct.Id));
                return Ok(updatedProduct);
            }
            else
                return BadRequest();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var result = await _mediator.Send(new ReadProductQuery(p => p.Id == id));
            if (result is null)
                return BadRequest();

            await _mediator.Send(new DeleteProductCommand(id));
            return NoContent();
        }
    }
}
