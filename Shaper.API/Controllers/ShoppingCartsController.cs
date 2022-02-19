﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shaper.API.RequestHandlers;
using Shaper.API.RequestHandlers.IRequestHandlers;
using Shaper.DataAccess.Repo.IRepo;
using Shaper.Models.Entities;
using Shaper.Models.Models.ShoppingCartModels;
using System.Data;

namespace Shaper.API.Controllers
{
    [Route("api/[controller]")]
    [Authorize(Roles = "Customer")]

    [ApiController]
    public class ShoppingCartsController : ControllerBase
    {
        private readonly IUnitOfWork _db;
        private readonly IRequestHandler _requestHandler;

        public ShoppingCartsController(IUnitOfWork db, IRequestHandler requestHandler)
        {
            _db = db;
            _requestHandler = requestHandler;
        }


        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetUserShoppingCartById(int id)
        {
            var result = await _db.ShoppingCarts.GetFirstOrDefaultAsync(x => x.Id == id);
            return Ok(new ShoppingCartSimpleModel(result));
        }



        [HttpPost]
        public async Task<IActionResult> GetUserShoppingCart(ShoppingCartRequestModel user)
        {
            var shoppingCart = await _requestHandler.ShoppingCarts.GetUserShoppingCartAsync(user.Identity);
            if (shoppingCart is null)
                return NotFound(new { message = "User does not have a shopping cart to process." });
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
                var productDetails = await _db.Products.GetFirstOrDefaultAsync(x => x.Id == cartProductModel.ProductId);
                if (productDetails is null)
                    return BadRequest(new { message = "The product you're trying to add to the cart does not exist." });

                CartProduct cartProduct = new CartProduct();
                //ShoppingCart
                var shoppingcart = await _requestHandler.ShoppingCarts.ShoppingCartExistAsync(cartProductModel.ShaperCustomer);
                if (shoppingcart is null)
                    shoppingcart = await _requestHandler.ShoppingCarts.GetFreshShoppingCartAsync(cartProductModel.ShaperCustomer);
                else
                    cartProduct = await _db.CartProducts.GetFirstOrDefaultAsync(x => x.ShoppingCartId == shoppingcart.Id && x.ProductId == productDetails.Id);

                //Add Or Update CartProduct
                if (cartProduct?.ProductId is 0 || cartProduct is null)
                    await _requestHandler.ShoppingCarts.AddNewCartProductAsync(cartProductModel, productDetails.Price, shoppingcart.Id);
                else
                    await _requestHandler.ShoppingCarts.UpdateCartProductAsync(cartProduct, cartProductModel.ProductQuantity);

                await _requestHandler.ShoppingCarts.CalulatingShoppingCartValue(shoppingcart);

                //Revize what this return.
                return Ok(shoppingcart);
            }
            else
            {
                return BadRequest();
            }
        }


        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateShoppingCart(int id, ShoppingCartUpdateModel cart)
        {
            await _requestHandler.ShoppingCarts.GetShoppingCartByIDAsync(id);
            if (cart is null)
                return BadRequest();

            await _requestHandler.ShoppingCarts.UpdateShoppingCartAsync(id, cart);
            return Ok();
        }


        [HttpDelete("{id:int}")]
        public async Task<IActionResult> RemoveShoppingCart(int id)
        {
            ShoppingCart result = await _requestHandler.ShoppingCarts.RemoveShoppingCart(id);
            if (result is null)
                return NotFound(new { message = "The shoppingcart you're trying to remove from the cart does not exist." });

            return Ok();
        }



        [HttpDelete("RemoveCartProduct")]
        public async Task<IActionResult> RemoveItemFromCart(CartProductDeleteModel cartProductModel)
        {
            if (ModelState.IsValid)
            {
                var productDetails = await _db.Products.GetFirstOrDefaultAsync(x => x.Name.ToLower() == cartProductModel.ProductName.ToLower());
                if (productDetails is null)
                    return BadRequest(new { message = "The product you're trying to remove from the cart does not exist." });

                var shoppingcart = await _requestHandler.ShoppingCarts.ShoppingCartExistAsync(cartProductModel.ShaperCustomer);
                if (shoppingcart is null)
                    return BadRequest(new { message = "We are not able to remove this item from this users ShoppingCart since the user does not have an active shoppingcart." });

                await _requestHandler.ShoppingCarts.RemoveItemFromShoppingCartAsync(shoppingcart.Id, productDetails.Id);

                await _requestHandler?.ShoppingCarts.CalulatingShoppingCartValue(shoppingcart);

                return Ok();
            }
            else
                return BadRequest();
        }

    }
}
