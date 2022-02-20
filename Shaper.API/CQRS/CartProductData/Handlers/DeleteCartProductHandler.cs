using MediatR;
using Microsoft.EntityFrameworkCore;
using Shaper.API.CQRS.CartProductData.Commands;
using Shaper.DataAccess.Context;
using Shaper.Models.Entities;

namespace Shaper.API.CQRS.CartProductData.Handlers
{
    //public class DeleteCartProductHandler : IRequestHandler<DeleteCartProductCommand, CartProduct>
    //{

    //    private readonly AppDbContext _db;

    //    public DeleteCartProductHandler(AppDbContext db)
    //    {
    //        _db = db;
    //    }

    //    public async Task<CartProduct> Handle(DeleteCartProductCommand request, CancellationToken cancellationToken)
    //    {
    //        //var color = await _db.CartProducts.FirstOrDefaultAsync();
    //        //_db.Remove(color);
    //        //await _db.SaveChangesAsync();
    //        //return color;
    //    }
    //}
}
