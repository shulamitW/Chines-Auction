using Chines_auction_project.DAL;
using Chines_auction_project.Modells;

namespace Chines_auction_project.BLL
{
    public class UserService: IUserService
    {
        private readonly IUserDal UserDal;
        public UserService(IUserDal userDal)
        {
            UserDal = userDal;
        }
        public async Task<List<User>>  GetAllusers()
        {
            return await UserDal.GetAllusers();
        }
        public async Task<User> Register(User user)
        {
            return await UserDal.Register(user);
        }
        public async Task<string> Login(string userName, string password)
        {
            return await UserDal.Login(userName,password);
        }
        public async Task<User> RemoveUser(int id)
        {
            return await UserDal.RemoveUser(id);

        }
        public async Task<User> UpdateUser(User user, int id)
        {
            return await UserDal.UpdateUser(user,id);

        }
    }
}
