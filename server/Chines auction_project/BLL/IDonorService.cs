using Chines_auction_project.Modells;
using Chines_auction_project.Modells.Dto;
using Microsoft.AspNetCore.Mvc;

namespace Chines_auction_project.BLL
{
    public interface IDonorService
    {
        Task<List<Donor>> GetDonors();
        Task<Donor> GetDonorById(int donorId);
        Task<Donor> AddDonor(Donor donor);
        Task<Donor> RemoveDonor(int id);
        Task<Donor> UpdateDonor(Donor donor, int id);
        Task<List<Donor>> SearchByEmail(string email);
        Task<List<Donor>> SearchByName(string Name);
        //public List<Donor> SearchByPresent(string present);



    }
}
