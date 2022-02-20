﻿using MediatR;
using Shaper.Models.Entities;
using Shaper.Models.Models.ShoppingCartModels;

namespace Shaper.API.CQRS.ShoppingCartData.Commands
{
    public record CalculateShoppingCartCommand(ShoppingCart ShoppingCart) : IRequest<ShoppingCart>;
}
