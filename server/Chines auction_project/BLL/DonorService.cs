using Chines_auction_project.DAL;
using Chines_auction_project.Modells;
using Chines_auction_project.Modells.Dto;
using Microsoft.AspNetCore.Mvc;

namespace Chines_auction_project.BLL
{
    public class DonorService:IDonorService
    {
        private readonly IDonorDal DonorDal;
        public DonorService(IDonorDal DonorDal)
        {
            this.DonorDal = DonorDal;
        }
        public async Task<List<Donor>> GetDonors()
        {
            return await DonorDal.GetDonors();
        }
        public async Task<Donor> GetDonorById(int donorId)
        {
            return await DonorDal.GetDonorById(donorId);
        }
        public async Task<Donor> AddDonor(Donor donor)
        {
            return await DonorDal.AddDonor(donor);
        }
        public async Task<Donor> RemoveDonor(int id)
        {
            return await DonorDal.RemoveDonor(id);
        }
        public async Task<Donor> UpdateDonor(Donor donor, int id)
        {
            return await DonorDal.UpdateDonor(donor,id);

        }
        public async Task<List<Donor>> SearchByEmail(string email)
        {
            return await DonorDal.SearchByEmail(email);
        }
        public async Task<List<Donor>> SearchByName(string Name)
        {
            return await DonorDal.SearchByName(Name);

        }
        //public List<Donor> SearchByPresent(string present)
        //{
        //    return DonorDal.SearchByPresent(present);
        //}

    }
}
