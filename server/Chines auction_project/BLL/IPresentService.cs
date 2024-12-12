using Chines_auction_project.Modells;

namespace Chines_auction_project.BLL
{
    public interface IPresentService
    {
        Task<List<Present>> GetAllPresent();
        Task<Present> AddPresent(Present present);
        Task<Present> RemovePresent(int id);
        Task<Present> UpdatePresent(Present present, int id);
        Task<Donor> GetDonor(int present);
        Task<Present> UpdateWinner(int presentId, int userId);
        Task<List<Present>> SearchByName(string name);
        Task<List<Present>> SearchByDonor(string donor);
        //Task<List<Present>> SearchByNumOfPurcheses(int numOfPurcheses);

        Task<List<Present>> SearchByNumOfPurchases(int numOfPurchases);


    }
}
