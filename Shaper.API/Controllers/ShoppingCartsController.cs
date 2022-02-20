using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shaper.API.CQRS.CartProductData.Commands;
using Shaper.API.CQRS.CartProductData.Queries;
using Shaper.API.CQRS.ColorData.Queries;
using Shaper.API.CQRS.ProductData.Queries;
using Shaper.API.CQRS.ShoppingCartData.Commands;
using Shaper.API.CQRS.ShoppingCartData.Queries;
using Shaper.API.RequestHandlers;
using Shaper.API.RequestHandlers.IRequestHandlers;
using Shaper.DataAccess.Repo.IRepo;
using Shaper.Models.Entities;
using Shaper.Models.Models.ShoppingCartModels;
using System.Data;

namespace Shaper.API.Controllers
{
    [Route("api/[controller]")]
    //[Authorize(Roles = "Customer")]

    [ApiController]
    public class ShoppingCartsController : ControllerBase
    {
        private readonly IMediator _mediator;


        public ShoppingCartsController(IMediator mediator)
        {
            _mediator = mediator;
        }


        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetUserShoppingCartById(int id)
        {
            var result = await _mediator.Send(new ReadShoppingCartQuery(x => x.Id == id));
            return Ok(new ShoppingCartSimpleModel(result));
        }


        [HttpPost]
        public async Task<IActionResult> GetUserShoppingCart(ShoppingCartRequestModel user)
        {
            var shoppingCart = await _mediator.Send(new ReadDetailedShoppingCartQuery(user.Identity));
            if (shoppingCart is null)
                return NotFound(new { message = "User does not have an active shopping cart to process." });

            if (shoppingCart.CartProducts is null || shoppingCart.CartProducts?.Count < 1)
                return NotFound(new { message = "There is a shoppingcart but its empty." });

            var shoppingCartModel = new UserShoppingCartModel(shoppingCart);
            return Ok(shoppingCartModel);
        }

        [HttpPost("AddToCart")]
        public async Task<IActionResult> AddItemToCart(CartProductAddModel cartProductModel)
        {
            if (ModelState.IsValid)
            {
                //Product
                var productDetails = await _mediator.Send(new ReadProductQuery(x => x.Id == cartProductModel.ProductId));
                if (productDetails is null)
                    return BadRequest(new { message = "The product you're trying to add to the cart does not exist." });

                CartProduct cartProduct = new CartProduct();

                //ShoppingCart
                var shoppingcart = await _mediator.Send(new ReadShoppingCartQuery(x => x.CustomerIdentity == cartProductModel.ShaperCustomer && x.CheckedOut == false));
                if (shoppingcart is null)
                    shoppingcart = await _mediator.Send(new CreateNewShoppingCartCommand(cartProductModel.ShaperCustomer));
                else
                    cartProduct = await _mediator.Send(new ReadCartProductQuery(x => x.ShoppingCartId == shoppingcart.Id && x.ProductId == productDetails.Id));

                //Add Or Update CartProduct
                if (cartProduct?.ProductId is 0 || cartProduct is null)
                    await _mediator.Send(new AddProductToShoppingCartCommand(cartProductModel, productDetails.Price, shoppingcart.Id));
                else
                    await _mediator.Send(new UpdateCartProductCommand(cartProduct, cartProductModel.ProductQuantity));

                await _mediator.Send(new CalculateShoppingCartCommand(shoppingcart));
                return Ok(shoppingcart);
            }
            else
                return BadRequest();
        }


        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateShoppingCart(int id, ShoppingCartUpdateModel cart)
        {
            var result = await _mediator.Send(new ReadShoppingCartQuery(x=>x.Id == id));
            if (result is null)
                return BadRequest();

            await _mediator.Send(new UpdateShoppingCartCommand(cart, id));
            return Ok();
        }


        [HttpDelete("{id:int}")]
        public async Task<IActionResult> RemoveShoppingCart(int id)
        {
            ShoppingCart result = await _mediator.Send(new DeleteShoppingCartCommand(id));
            if (result is null)
                return NotFound(new { message = "The shoppingcart you're trying to remove from the cart does not exist." });

            return Ok();
        }



        [HttpDelete("RemoveCartProduct")]
        public async Task<IActionResult> RemoveItemFromCart(CartProductDeleteModel cartProductModel)
        {
            if (ModelState.IsValid)
            {
                var productDetails = await _mediator.Send(new ReadProductQuery(x => x.Name.ToLower() == cartProductModel.ProductName.ToLower()));
                if (productDetails is null)
                    return BadRequest(new { message = "The product you're trying to remove from the cart does not exist." });

                var shoppingcart = await _mediator.Send(new ReadCurrentShoppingCartQuery(cartProductModel.ShaperCustomer));
                if (shoppingcart is null)
                    return BadRequest(new { message = "We are not able to remove this item from this users ShoppingCart since the user does not have an active shoppingcart." });

                await _mediator.Send(new DeleteProductFromShoppingCartCommand(shoppingcart.Id, productDetails.Id));
                await _mediator.Send(new CalculateShoppingCartCommand(shoppingcart));
                return Ok();
            }
            else
                return BadRequest();
        }

    }
}
