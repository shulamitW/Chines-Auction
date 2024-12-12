using Chines_auction_project.Modells;
using Chines_auction_project.Modells.Dto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Xml.Linq;

namespace Chines_auction_project.DAL
{
    public class DonorDal:IDonorDal
    {
        private readonly AuctionContex auctionContex;
        public DonorDal(AuctionContex auctionContex)
        {
            this.auctionContex = auctionContex;
        }
        public async Task<List<Donor>> GetDonors()
        {
            try
            {
                return await auctionContex.Donor.Include(c=>c.Presents).ThenInclude(p=>p.Category).ToListAsync();
            }
            catch (Exception)
            {

                throw;
            }
        }
        public async Task<Donor> GetDonorById(int donorId )
        {
            try
            {
                return await auctionContex.Donor.FirstOrDefaultAsync(d=>d.Id == donorId);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<Donor> AddDonor(Donor donor)
        {
            try
            {
                await auctionContex.Donor.AddAsync(donor);
                await auctionContex.SaveChangesAsync();
                return donor;


            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public async Task<Donor> RemoveDonor(int id)
        {
            try
            {
                var donor= await auctionContex.Donor.FirstOrDefaultAsync(c => c.Id == id);
                if (donor == null)
                {
                    throw new Exception($"donor {id} not found");
                }
                auctionContex.Donor.Remove(donor);
                await auctionContex.SaveChangesAsync();
                return donor;


            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<Donor> UpdateDonor(Donor donor, int id)
        {
            try
            {
                donor.Id = id;
                var donor1 = await auctionContex.Donor.FirstOrDefaultAsync(c => c.Id == id);
                if (donor1 == null)
                {
                    throw new Exception($"donor {id} not found");
                }
                //בדיקות אם לא נל 
                //if(donor.FullName != "string") donor1.FullName = donor.FullName;
                if (donor.FirstName != "string") donor1.FirstName = donor.FirstName;
                if (donor.LastName != "string") donor1.LastName = donor.LastName;
                if (donor.LastName != "string" || donor.FirstName != "string")
                {
                    if (donor.LastName != "string" && (donor.FirstName == "string"))
                        donor1.FullName = donor.FirstName+' '+donor1.LastName;
                    if (donor.LastName == "string" && (donor.FirstName != "string"))
                        donor1.FullName = donor1.FirstName + ' ' + donor.LastName;
                    else
                        donor1.FullName = donor1.FirstName + ' ' + donor1.LastName;
                }
                if (donor.Address != "string") donor1.Address = donor.Address;
                if (donor.Email != "string") donor1.Email = donor.Email;
                if (donor.Phone != "string") donor1.Phone = donor.Phone;
                if (donor.ImagePath != "string") donor1.ImagePath = donor.ImagePath;

                //auctionContex.Donor.Update(donor);
                await auctionContex.SaveChangesAsync();
                return donor1;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public async Task<List<Donor>> SearchByEmail(string email)
        {
            try
            {
                var donor = await auctionContex.Donor.Where(c => c.Email.Contains(email)).ToListAsync();

                //var donor = auctionContex.Donor.FirstOrDefault(c => c.Email == email);
                if (donor==null)
                    throw new Exception($"this email {email} not found");
                return donor;
            }
            catch (Exception e)
            {

                throw e;
            }
        }
        public async Task<List<Donor>> SearchByName(string Name)
        {
            try
            {
                var donor = await auctionContex.Donor.Where(c => c.FullName.Contains(Name)).ToListAsync();
                if (donor == null)
                    throw new Exception($"this name {Name} not found");
                return donor;

            }
            catch (Exception e)
            {

                throw e;
            }
        }
        //public List<Donor> SearchByPresent(string present)////////////////
        //{
        //    try
        //    {

        //        var p = auctionContex.Present.FirstOrDefault(c => c.Name == present);
        //        if (p == null)
        //            throw new Exception($"this present {present} not found");
        //        var donor = auctionContex.Donor.FirstOrDefault(c => c.Id == p.DonorId);
        //        if (p == null)
        //            throw new Exception($"this present {present} not found");
        //        return donor;

        //    }
        //    catch (Exception e)
        //    {

        //        throw e;
        //    }
        //}
    }
}
