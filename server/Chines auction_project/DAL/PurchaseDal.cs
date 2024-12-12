using AutoMapper;
using Chines_auction_project.Modells;
using Chines_auction_project.Modells.Dto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Drawing;
using System.Net.Sockets;

namespace Chines_auction_project.DAL
{
    public class PurchaseDal : IPurchaseDal
    {
        private readonly AuctionContex auctionContex;
        private readonly IMapper mapper;

        public PurchaseDal(AuctionContex auctionContex, IMapper mapper)
        {
            this.auctionContex = auctionContex;
            this.mapper = mapper;
        }
        public async Task<List<Purchase>> GetAllPurchase()
        {
            try
            {
                return await auctionContex.Purchase.Include(c => c.Tickets).ToListAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<Purchase>> GetAllPurchaseById(int userId)
        {
            try
            {
                var purchases = await auctionContex.Purchase.Where(c => c.UserId == userId & c.Status == true).ToListAsync();
                return purchases;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<Purchase> GetShoppingCartById(int userId)
        {
            try
            {
                var purchases = await auctionContex.Purchase.Include(c => c.Tickets).ThenInclude(p=>p.Present).ThenInclude(c=>c.Category).FirstOrDefaultAsync(c => c.UserId == userId & c.Status == false);
                if (purchases == null)
                {
                    var p = new PurchaseDto();
                    p.UserId = userId;
                    var emptyCart = mapper.Map<Purchase>(p);
                    await auctionContex.Purchase.AddAsync(emptyCart);
                    await auctionContex.SaveChangesAsync();
                    var d = emptyCart;
                    return emptyCart;
                }
                return purchases;
            }
            catch (Exception)
            {
                throw new Exception($"user {userId} not found");
            }
        }
        //public async Task<Purchase> Buy(Purchase purchase)//change thr status and pay
        //{
        //    try
        //    {
        //        //var purchases = await auctionContex.Purchase.FirstOrDefaultAsync(c => c.Id == purchase.Id & c.Status == false);
        //        if (purchase != null && purchase.Status == false)
        //        {
        //            purchase.Status = true;
        //            // Mark the entity as modified so that EF Core knows it needs to be updated in the database
        //            auctionContex.Entry(purchase).State = EntityState.Modified;
        //        }
        //        await auctionContex.SaveChangesAsync();
        //        return purchase;
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}
        public async Task<Purchase> Buy(int cartId)//change thr status and pay
        {
            try
            {
                var cart = await auctionContex.Purchase.FirstOrDefaultAsync(c => c.Id == cartId & c.Status == false);
                if (cart != null && cart.Status == false)
                {
                    cart.Status = true;
                    // Mark the entity as modified so that EF Core knows it needs to be updated in the database
                    auctionContex.Entry(cart).State = EntityState.Modified;
                }
                await auctionContex.SaveChangesAsync();
                return cart;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<Purchase> AddToCart(int cartId, int presentId, int userId)
        {
            try
            {
                var cart = await auctionContex.Purchase.Where(p => p.Id == cartId && p.Status == false)
                  .Include(t => t.Tickets)
                  .FirstOrDefaultAsync();
                if (cart==null)
                    throw new Exception("you can not remove a bought ticket");

                if (cart.Tickets == null || !cart.Tickets.Any())
                {
                    cart.Tickets = new List<Ticket>();
                }

                var existingTicket = cart.Tickets.FirstOrDefault(i => i.PresentId == presentId);

                if (existingTicket != null)
                {
                    existingTicket.Quantity++;
                }
                else
                {
                    var newTicket = new TicketDto
                    {
                        PresentId = presentId,
                        PurchaseId = cartId
                    };

                    var mappedTicket = mapper.Map<Ticket>(newTicket);

                    // Directly insert the new ticket into the Tickets list in myCart
                   // myCart.Tickets = myCart.Tickets.Concat(new List<Ticket> { mappedTicket }).ToList();

                    await auctionContex.Ticket.AddAsync(mappedTicket);

                    //var tmp = await auctionContex.Purchase.Where(p => p.Id == myCart.Id).FirstOrDefaultAsync();
                    cart.Tickets.Concat(new List<Ticket> { mappedTicket }).ToList();
                    await auctionContex.SaveChangesAsync();

                    //foreach (var ticket in auctionContex.Purchase)
                    //{
                    //    auctionContex.Entry(ticket).State = EntityState.Modified; // Set the state to Modified
                    //}
                }

                await auctionContex.SaveChangesAsync();

                return cart;
            }
            catch (Exception)
            {
                throw new Exception("An error occurred while adding to the cart.");
            }
        }

        public async Task<Purchase> RemoveFromCart(int cartId, int presentId, int userId)//remove from cart
        {
            try
            {
                var cart = await auctionContex.Purchase.Where(p => p.Id == cartId)
                  .Include(t => t.Tickets)
                  .FirstOrDefaultAsync();
                if(cart==null)
                    throw new Exception("cart not found");

                if (cart.Status == true)
                    throw new Exception("you can not remove a bought ticket");

                var existingTicket = cart.Tickets.FirstOrDefault(i => i.PresentId == presentId);
                if (existingTicket == null)
                    throw new Exception("Ticket not found");

                if (existingTicket.Quantity == 1)
                {
                    auctionContex.Ticket.Remove(existingTicket);
                }
                else
                {
                    existingTicket.Quantity--;
                }
                await auctionContex.SaveChangesAsync();

                return cart;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while removing from cart.", ex);
            }
        }


        public async Task<List<Purchase>> GetPurchaseByPresent(int presentId)
        {
            try
            {
                List<Purchase> purchases1 = await auctionContex.Purchase
                .Where(p => p.Tickets.Any(t => t.PresentId == presentId) && p.Status == true)
                .ToListAsync();
                return purchases1;
                //.Include(u=>u.User)
                //var p=await auctionContex.Purchase.Include(Ticket).Where(c => c.PresentId == present.Id)
            }
            catch (Exception)
            {
                throw;
            }
        }

        //public async Task<List<Purchase>> SortByNumOfPurchases()
        //{
        //    var result = await auctionContex.Ticket
        //        .Where(t => t.PurchaseId != null)
        //        .GroupBy(t => t.PresentId)
        //        .Select(g => new
        //        {
        //            PresentId = g.Key,
        //            PurchaseCount = g.Count(),
        //        })
        //        .ToListAsync();

        //    var presentDetails = auctionContex.Present.Include(c => c.Category)
        //              .AsEnumerable()
        //              .Join(result, p => p.Id, r => r.PresentId, (p, r) => p)
        //              .ToList();


        //    return presentDetails;
        //}
        public async Task<List<Present>> SortByNumOfPurchases()
        {
            // TO DO CHECK THE cart.Status
            var result = await auctionContex.Ticket
                .Where(t => t.PurchaseId != null)
                .GroupBy(t => t.PresentId)
                .Select(g => new
                {
                    PresentId = g.Key,
                    PurchaseCount = g.Count(),
                })
                .OrderByDescending(r => r.PurchaseCount)
                .ToListAsync();

            var presentIds = result.Select(r => r.PresentId);

            var presents = await auctionContex.Present
                .Include(p => p.Purchase)
                .ThenInclude(p => p.Tickets)
                .Where(p => p.Purchase.Any(purch => purch.Tickets.Any(t => t.PresentId != null && presentIds.Contains(t.PresentId))))
                .ToListAsync();

            return presents;
        }

        //public async Task<List<Purchase>> SortByNumOfPurchases()
        //{
        //    var result = await auctionContex.Ticket
        //        .Where(t => t.PurchaseId != null)
        //        .GroupBy(t => t.PresentId)
        //        .Select(g => new
        //        {
        //            PresentId = g.Key,
        //            PurchaseCount = g.Count(),
        //        })
        //        .OrderByDescending(r => r.PurchaseCount)
        //        .ToListAsync();

        //    var presentIds = result.Select(r => r.PresentId);

        //    //var purchases = await auctionContex.Purchase
        //    //    //.Include(p => p.User)
        //    //    .Include(p => p.Tickets)
        //    //    .Where(p => presentIds.Contains(p.Id))
        //    //    .ToListAsync();


        //    var purchases = await auctionContex.Purchase
        //    .Include(p => p.Tickets)
        //    .Where(p => p.Tickets.Any(t => t.PresentId != null && presentIds.Contains(t.PresentId.Value)))
        //    .ToListAsync();

        //    return purchases;
        //}
        // {
        //     try
        //     {
        //     var c = await auctionContex.Purchase.include(Ticket).Where(c => c.PresentId == present.id)
        //     var present = await auctionContex.Purchase.Where(c => c.PresentId == present.id);
        //     }
        //     catch(Exception)
        //     {
        //         throw;
        //     }
        //}

    }
}
