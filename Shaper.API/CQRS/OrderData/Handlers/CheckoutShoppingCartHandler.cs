using MediatR;
using Shaper.API.CQRS.OrderData.Commands;
using Shaper.API.CQRS.ShoppingCartData.Queries;
using Shaper.DataAccess.Context;
using Shaper.Models.Entities;
using System.Web.Helpers;

namespace Shaper.API.CQRS.OrderData.Handlers
{
    public class CheckoutShoppingCartHandler : IRequestHandler<CheckoutShoppingCartCommand, Task>
    {

        private readonly AppDbContext _db;
        private readonly IMediator _mediator;

        public CheckoutShoppingCartHandler(AppDbContext db, IMediator mediator)
        {
            _db = db;
            _mediator = mediator;
        }

        public async Task<Task> Handle(CheckoutShoppingCartCommand request, CancellationToken cancellationToken)
        {
            var userShoppingCart = await _mediator.Send(new ReadShoppingCartQuery(a => a.CustomerIdentity == request.UserIdentity && a.CheckedOut == false));
            userShoppingCart.CheckedOut = true;
            _db.ShoppingCarts.Update(userShoppingCart);
            await _db.SaveChangesAsync();
            return Task.CompletedTask;
        }
    }
}
