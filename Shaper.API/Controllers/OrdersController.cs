using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shaper.API.CQRS.ColorData.Queries;
using Shaper.API.CQRS.OrderData.Commands;
using Shaper.API.CQRS.OrderData.Queries;
using Shaper.API.RequestHandlers.IRequestHandlers;
using Shaper.DataAccess.Repo.IRepo;
using Shaper.Models.Models.OrderModels;

namespace Shaper.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IRequestHandler _requestHandler;
        private readonly IMediator _mediator;

        public OrdersController(IRequestHandler requestHandler, IMediator mediator)
        {
            _requestHandler = requestHandler;
            _mediator = mediator;
        }


        [HttpPost]
        public async Task<IActionResult> GetUserOrders(OrdersRequestModel user)
        {
            var orders = await _mediator.Send(new ReadFilteredOrdersQuery(x => x.CustomerIdentity == user.Identity));
            if (orders is null)
                return NotFound(new { message = "This user does not have any Orders with us." });

            return Ok(orders);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetUserOrder(int id, OrdersRequestModel orderreq)
        {
            if(id == 0 || id != orderreq.OrderId)
                return BadRequest();

            var order = await _mediator.Send(new ReadOrderQuery(x=>x.Id == orderreq.OrderId));
            if (order is null)
                return NotFound(new { message = "The order that you're trying retrieve is not with us." });

            return Ok(order);
        }


        [HttpPost("PlaceOrder")]
        public async Task<IActionResult> PlacingOrder(OrdersRequestModel user)
        {
            var userShoppingCart = await _mediator.Send(new ReadDetailedShoppingCartQuery(user.Identity));
            if (userShoppingCart is null || userShoppingCart.CartProducts.Count is 0)
                return NotFound(new { message = "User does not have a shopping cart to process." });
            
            var order = _mediator.Send(new CreateOrderCommand(userShoppingCart));
            return Ok(order);
        }

        [HttpPut]
        public async Task<IActionResult> PutUserOrders(OrderUpdateModel order)
        {
            if (ModelState.IsValid)
            {
                var originalOrder = await _mediator.Send(new ReadOrderQuery(x=>x.Id==order.OrderId));
                if (originalOrder is null)
                    return NotFound(new { message = $"Order with ID: {order.OrderId} does not exist." });

                await _mediator.Send(new UpdateOrderCommand(order, originalOrder));
                return Ok();
            }
            return BadRequest();
        }

        [HttpDelete("{orderid:int}")]
        public async Task<IActionResult> DeleteOrder(int orderid)
        {
            if (ModelState.IsValid)
            {
                var order = await _mediator.Send(new ReadOrderQuery(x => x.Id == orderid));
                if (order is null)
                    return NotFound(new { message = $"We cannot find the with Order ID: {orderid}." });

                await _mediator.Send(new DeleteOrderCommand(orderid));
                return Ok(new { message = "Entry was deleted successfully." });
            }
            return BadRequest();
        }
    }
}
