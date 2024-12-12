using AutoMapper;
using Chines_auction_project.BLL;
using Chines_auction_project.Modells;
using Chines_auction_project.Modells.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Chines_auction_project.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class UserController:ControllerBase
    {
        private readonly IMapper mapper;
        private readonly IUserService userService;
        public UserController(IMapper mapper, IUserService userService)
        {
            this.mapper = mapper;
            this.userService = userService;
        }

        [AllowAnonymous]
        [HttpGet("GetAllUsers")]
       public async Task<ActionResult<User>> GetAllusers()
        {
            var users=await userService.GetAllusers();
            return users == null ? NotFound() : Ok(users);
        }

        [AllowAnonymous]
        [HttpPost("Register")]
        public async Task<ActionResult<User>> Register(UserDto user)
        {
            var u = mapper.Map<User>(user);
            return Created($"http://localhost:3000/user/{u.Id}", await userService.Register(u));
        }
        //[HttpPost("Login")]

        //public async Task<ActionResult<object>> Login(string userName, string password)
        //{
        //   // var u = mapper.Map<User>(user);
        //    return Created($"http://localhost:3000/user/{userName}", await userService.Login(userName,password));
        //}
        //[Authorize(Roles = "Manager")]
        [AllowAnonymous]
        [HttpPost("Login")]
        public async Task<ActionResult> Login([FromForm] string userName, [FromForm] string password)
        {
            var result = await userService.Login(userName, password);
            if (result != null)
            {
                return Ok(result);
            }
            return Unauthorized();
        }
        [Authorize(Roles = "Manager")]
        [HttpDelete("RemoveUser/{id}")]
        public async Task<ActionResult<User>> RemoveUser(int id)
        {
            return id == null ? NotFound() : Ok(await userService.RemoveUser(id));
        }

        [Authorize(Roles = "Manager")]
        [HttpPut("UpdateUser/{id}")]
        public async Task<ActionResult<User>> UpdateUser(UserDto user, int id)
        {
            var u = mapper.Map<User>(user);
            return u == null ? NotFound() : Ok(await userService.UpdateUser(u, id));
        }

    }
    
}
