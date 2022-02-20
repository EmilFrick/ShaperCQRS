﻿using MediatR;
using Shaper.Models.Entities;
using System.Linq.Expressions;

namespace Shaper.API.CQRS.ColorData.Queries
{
    public record ReadDetailedShoppingCartQuery(string userIdentity) : IRequest<ShoppingCart>;
}
