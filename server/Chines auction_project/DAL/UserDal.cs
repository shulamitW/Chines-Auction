using Chines_auction_project.Modells;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using System.Drawing;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using JwtRegisteredClaimNames = System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames;
using Microsoft.AspNetCore.Authorization;

namespace Chines_auction_project.DAL
{
    public class UserDal : IUserDal
    {
        private readonly AuctionContex auctionContex;
        private readonly IConfiguration configuration;
        public UserDal(AuctionContex auctionContex, IConfiguration configuration)
        {
            this.auctionContex = auctionContex;
            this.configuration = configuration;

        }
        public async Task<List<User>> GetAllusers() 
        {
            try
            {
                return await auctionContex.User.ToListAsync();
            }
            catch (Exception)
            {

                throw;
            }
        }
        public async Task<User> Register(User user)
        {
            try
            {
                var exist = await auctionContex.User.FirstOrDefaultAsync(c => c == user);
                if (exist != null)
                {
                   
                }
                await auctionContex.User.AddAsync(user);
                await auctionContex.SaveChangesAsync();
                return user;
            }
            catch (Exception)
            {

                throw;
            }
        }
        [AllowAnonymous]
        public async Task<string> Login(string userName, string password)
        {
            try
            {
                var user = await auctionContex.User.FirstOrDefaultAsync(u => u.UserName == userName && u.Password == password);
                if (user != null)
                {
                    var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]));
                    var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
                    var claims = new[]
                    {
                        new Claim("UserID", user.Id.ToString()),
                        new Claim("role", "User"),
                        new Claim(ClaimTypes.NameIdentifier, user.FullName),
                        new Claim(ClaimTypes.Email, user.Email),
                        new Claim(ClaimTypes.OtherPhone, user.Phone),
                        new Claim(ClaimTypes.Role, "User")
                    };
                    var token = new JwtSecurityToken(configuration["Jwt:Issuer"],
                                                    configuration["Jwt:Audience"],
                                                    claims,
                                                    expires: DateTime.Now.AddMinutes(15),
                                                    signingCredentials: credentials);
                        
                    string tokenValue=new JwtSecurityTokenHandler().WriteToken(token);
                   
                    return tokenValue;
                }
                else
                {
                    var manager = await auctionContex.Manager.FirstOrDefaultAsync(u => u.UserName == userName && u.Password == password);
                    if (manager != null)
                    {
                        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]));
                        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
                        var claims = new[]
                        {
                        new Claim("UserID", manager.Id.ToString()),
                        new Claim("role", "Manager"),
                        new Claim(ClaimTypes.NameIdentifier, manager.FullName),
                        new Claim(ClaimTypes.Email, manager.Email),
                        new Claim(ClaimTypes.OtherPhone, manager.Phone),
                        new Claim(ClaimTypes.Role, "Manager")
                    };
                        var token = new JwtSecurityToken(configuration["Jwt:Issuer"],
                                                        configuration["Jwt:Audience"],
                                                        claims,
                                                        expires: DateTime.Now.AddMinutes(15),
                                                        signingCredentials: credentials);

                        string tokenValue = new JwtSecurityTokenHandler().WriteToken(token);

                        return tokenValue;
                    }
                    throw new Exception("User does not exist, please register");
                }
            
                
            }
            catch (Exception ex)
            {
                // Log exception here if necessary
                throw new Exception("An error occurred during login", ex);
            }
        }


        public async Task<User> RemoveUser(int id)
        {
            try
            {
                var user = await auctionContex.User.FirstOrDefaultAsync(c => c.Id == id);
                if (user == null)
                {
                    throw new Exception($"user {id} not found");
                }
                auctionContex.User.Remove(user);
                await auctionContex.SaveChangesAsync();
                return user;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public async Task<User> UpdateUser(User user, int id)
        {
            user.Id = id;
            var u = await auctionContex.User.FirstOrDefaultAsync(c => c.Id == id);
            if (u == null)
            {
                throw new Exception($"donor {id} not found");
            }
            //בדיקות אם לא נל 
            if (user.FullName != "string") u.FullName = user.FullName;
            if (user.Address != "string") u.Address = user.Address;
            if (user.Email != "string") u.Email = user.Email;
            if (user.Phone != "string") u.Phone = user.Phone;
            if (user.Password != "string") u.Phone = user.Phone;
            if (user.UserName != "string") u.UserName = user.UserName;

            //auctionContex.Donor.Update(donor);
            await auctionContex.SaveChangesAsync();
            return u;
        }
    }
}
