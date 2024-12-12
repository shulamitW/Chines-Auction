using Chines_auction_project.DAL;
using Chines_auction_project.Modells;

namespace Chines_auction_project.BLL
{
    public class PresentService : IPresentService
    {
        private readonly IPresentDal PresentDal;
      
        public PresentService(IPresentDal PresentDal) {
            this.PresentDal = PresentDal;
        }
        public async Task<List<Present>> GetAllPresent()
        {
            return await PresentDal.GetAllPresent();
        }
        public async Task<Present> AddPresent(Present present)
        {
            return await PresentDal.AddPresent(present);  
        }
        public async Task<Present> RemovePresent(int id)
        {
            return await PresentDal.RemovePresent(id);
        }
        public async Task<Present> UpdatePresent(Present present, int id)
        {
            return await PresentDal.UpdatePresent(present, id);
        }
        public async Task<Donor> GetDonor(int present)
        {
            return await PresentDal.GetDonor(present);
        }
        public async Task<Present> UpdateWinner(int presentId, int userId)
        {
            return await PresentDal.UpdateWinner(presentId, userId);

        }
        public async Task<List<Present>> SearchByName(string name)
        {
            return await PresentDal.SearchByName(name);
        }

        public async  Task<List<Present>> SearchByDonor(string donor)
        {
            return await PresentDal.SearchByDonor(donor);
        }
        public async Task<List<Present>> SearchByNumOfPurchases(int numOfPurcheses)
        {
            return await PresentDal.SearchByNumOfPurchases(numOfPurcheses);
        }
    }
}
