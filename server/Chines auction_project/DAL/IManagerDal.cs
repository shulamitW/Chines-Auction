using Chines_auction_project.Modells;

namespace Chines_auction_project.DAL
{
    public interface IManagerDal
    {
        Task<List<Manager>> GetAllManagers();
        Task<Manager> AddManager(Manager manager);
        Task<Manager> RemoveManager(int id);
        Task<Manager> UpdateManager(Manager manager, int id);
    }
}