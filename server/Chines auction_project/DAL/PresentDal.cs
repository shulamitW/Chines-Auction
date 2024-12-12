using Chines_auction_project.Modells;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Drawing;
using System.Xml.Linq;
using static Azure.Core.HttpHeader;

namespace Chines_auction_project.DAL
{
    public class PresentDal : IPresentDal
    {
        private readonly AuctionContex auctionContex;
        public PresentDal(AuctionContex auctionContex)
        {
            this.auctionContex = auctionContex;
        }
        public async Task<List<Present>> GetAllPresent()
        {
            try
            {
                //var presents = await auctionContex.Present.Include(p => p.Purchase)
                //    .ThenInclude(c=>c.Tickets)
                //    .ThenInclude(o=>o.Present).
                //    Include(c => c.Category)
                //    .Include(i=>i.Winner).ToListAsync();


                var presents = await auctionContex.Present
                  .Include(p => p.Purchase) 
                  .ThenInclude(c => c.Tickets) 
                  .Include(c => c.Category)
                  .Include(i => i.Winner)
                  .ToListAsync();
 
                foreach (var present in presents)
                {
                    present.Donor = await auctionContex.Donor.FirstOrDefaultAsync(d => d.Id == present.DonorId);
                }
                //return await auctionContex.Present.Include(c => c.Category).Include(p => p.Donor).ToListAsync();
                return presents;
            }
            catch (Exception)
            {
                throw;
            }
        }

        
        //public async Task<List<Present>> GetAllPresent()
        //{
        //    try
        //    {
        //        var presents = await auctionContex.Present
        //            .Include(c => c.Category)
        //            .ToListAsync();

        //        foreach (var present in presents)
        //        {
        //            auctionContex.Entry(present).Reference(p => p.Donor).Load();
        //        }

        //        return presents;
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}
        public async Task<Present> AddPresent(Present present)
        {
            try
            {
                await auctionContex.Present.AddAsync(present);
                await auctionContex.SaveChangesAsync();
                return present;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public async Task<Present> RemovePresent(int id)
        {
            try
            {
                var present = await auctionContex.Present.FirstOrDefaultAsync(c => c.Id == id);
                if (present == null)
                {
                    throw new Exception($"present {id} not found");

                }
                auctionContex.Present.Remove(present);//why cant be remove async??
                await auctionContex.SaveChangesAsync();
                return present;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public async Task<Present> UpdatePresent(Present present, int id)
        {
            try
            {
                present.Id = id;
                var p = await auctionContex.Present.FirstOrDefaultAsync(c => c.Id == id);
                if (p == null)
                {
                    throw new Exception($"present {id} not found");

                }
                if (present.Description != "string") p.Description = present.Description;

                if (present.WinnerId != 0) p.WinnerId = present.WinnerId;

                if (present.DonorId != 0) p.DonorId = present.DonorId;

                if (present.Price != 0) p.Price = present.Price;

                if (present.Name != "string") p.Name = present.Name;

                if (present.CategoryId != 0) p.CategoryId = present.CategoryId;
                if (present.ImagePath != null) p.ImagePath = present.ImagePath;


                await auctionContex.SaveChangesAsync();
                return p;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<Present> UpdateWinner(int presentId, int userId)
        {
            try
            {
                var p = await auctionContex.Present.FirstOrDefaultAsync(c => c.Id == presentId);
                if (userId != null)
                {
                    p.WinnerId = userId;
                }
                await auctionContex.SaveChangesAsync();
                return p;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<Donor> GetDonor(int present)
        {
            try
            {
                var p = await auctionContex.Present.FirstOrDefaultAsync(c => c.Id == present);
                var donor1 = await auctionContex.Donor.FirstOrDefaultAsync(d => d.Id == p.DonorId);
                return donor1;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<List<Present>> SearchByName(string name)
        {
            try
            {
                var present = await auctionContex.Present.Where(c => c.Name.Contains(name)).Include(o => o.Category).ToListAsync();
                if (present == null)
                    throw new Exception($"present {name} not found");
                return present;


            }
            catch (Exception)
            {

                throw;
            }
        }
        public async Task<List<Present>> SearchByDonor(string donor)////
        {
            try
            {
                //added include Category 
                var presents = await auctionContex.Present.Include(c => c.Donor).Where(p => p.Donor.FullName.Contains(donor)).Include(o => o.Category).ToListAsync();

                // var present = auctionContex.Present.FirstOrDefault(c => c.donorId == d.Id);
                if (presents == null)
                    throw new Exception($"donor {donor} not found");
                return presents;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<Present>> SearchByNumOfPurchases(int numOfPurchases)
        {
            var result = await auctionContex.Ticket
                .Where(t => t.PurchaseId != null)
                .GroupBy(t => t.PresentId)
                .Select(g => new
                {
                    PresentId = g.Key,
                    PurchaseCount = g.Count(),
                })
                .Where(g => g.PurchaseCount == numOfPurchases)
                .ToListAsync();

            var presentDetails = auctionContex.Present.Include(c => c.Category)
                      .AsEnumerable()
                      .Join(result, p => p.Id, r => r.PresentId, (p, r) => p)
                      .ToList();


            return presentDetails;
        }
   


    //public Task<Present> SearchByNumOfPurcheses(int numOfPurcheses)
    //    {
    //        try
    //        {

    //            var d = auctionContex.Purchase.Include(p=> p.Tickets).ThenInclude(c=>c.Present).;

    //            var present = auctionContex.Present.FirstOrDefault(c => c.donorId == d.Id);
    //            if (present == null)
    //                throw new Exception($"donor {donor} not found");
    //            return present;

    //        }
    //        catch (Exception)
    //        {

    //            throw;
    //        }
    //    }
    //public Present SearchByPrice(float price);

}
}
