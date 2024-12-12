using Chines_auction_project.Modells;
using Microsoft.EntityFrameworkCore;

namespace Chines_auction_project.DAL
{
    public class ManagerDal:IManagerDal
    {
        private readonly AuctionContex auctionContex;
        public ManagerDal(AuctionContex auctionContex)
        {
            this.auctionContex = auctionContex;
                
        }
        public async Task<List<Manager>> GetAllManagers()
        {
            try
            {
                return await auctionContex.Manager.ToListAsync();
            }
            catch (Exception)
            {

                throw;
            }
        }
        public async Task<Manager> AddManager(Manager manager)
        {
            try
            {
                await auctionContex.AddAsync(manager);
                await auctionContex.SaveChangesAsync();
                return manager;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public async Task<Manager> RemoveManager(int id)
        {
            try
            {
                var manager = await auctionContex.Manager.FirstOrDefaultAsync(c => c.Id == id);
                if (manager == null)
                {
                    throw new Exception($"manager {id} not found");

                }
                auctionContex.Manager.Remove(manager);
                await auctionContex.SaveChangesAsync();
                return manager;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public async Task<Manager> UpdateManager(Manager manager, int id)
        {
            manager.Id = id;
            var p = await auctionContex.Manager.FirstOrDefaultAsync(c => c.Id == id);
            if (p == null)
            {
                throw new Exception($"manager {id} not found");

            }
            if (manager.FullName != "string") p.FullName = manager.FullName;
            if (manager.UserName != "string") p.UserName = manager.UserName;
            if (manager.Password != "string") p.Password = manager.Password;
            if (manager.Email != "string") p.Email = manager.Email;
            if (manager.Phone != "string") p.Phone = manager.Phone;
            if (manager.Address != "string") p.Address = manager.Address;

            await auctionContex.SaveChangesAsync();
            return p;
        }
    }
}
