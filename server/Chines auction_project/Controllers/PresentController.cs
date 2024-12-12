using AutoMapper;
using Chines_auction_project.BLL;
using Chines_auction_project.DAL;
using Chines_auction_project.Modells;
using Chines_auction_project.Modells.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Drawing;

namespace Chines_auction_project.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PresentController:ControllerBase
    {
        public readonly IPresentService presentService;
        private readonly IMapper mapper;

        public PresentController(IPresentService presentService, IMapper mapper)
        {
            this.presentService = presentService;
            this.mapper = mapper;
        }
        [AllowAnonymous]
        [HttpGet("GetPresents")]
        public async Task<ActionResult<Present>> GetAllPresent()
        {
            var presents = await presentService.GetAllPresent();
            return presents == null ? NotFound() : Ok(presents);
        }
        [Authorize(Roles = "Manager")]
        [HttpPost("AddPresents")]
        public async Task<ActionResult<Present>> AddPresent(PresentDto present)
        {
            var p = mapper.Map<Present>(present);
            return Created($"http://localhost:3000/Present/{p.Id}", await presentService.AddPresent(p));
            //return PresentDal.AddPresent(present);
        }
        [Authorize(Roles = "Manager")]
        [HttpDelete("RemovePresent/{id}")]
        public async Task<ActionResult<Present>> RemovePresent(int id)
        {
            return id == null ? NotFound() : Ok(await presentService.RemovePresent(id));
        }
        [Authorize(Roles = "Manager")]
        [HttpPut("UpdatePresent/{id}")]
        public async Task<ActionResult<Present>> UpdatePresent(PresentDto present, int id)
        {
            var p = mapper.Map<Present>(present);
            return p == null ? NotFound() : Ok(await presentService.UpdatePresent(p, id));
        }
        [AllowAnonymous]
        [HttpGet("GetDonor/{presentId}")]
        public async Task<ActionResult<Donor>> GetDonor(int present)
        {
            return present == null ? NotFound() : Ok(await presentService.GetDonor(present));
        }
        [Authorize(Roles = "Manager")]
        [HttpPut("UpdateWinner/{userId}")]
        public async Task<ActionResult<Donor>> UpdateWinner(int presentId, int userId)
        {
            return presentId == null ? NotFound() : Ok(await presentService.UpdateWinner(presentId, userId));
        }


        [HttpGet("SearchByName")]
        public async Task<ActionResult<Present>> SearchByName(string name)
        {
            return name == null ? NotFound() : Ok(await presentService.SearchByName(name));
        }
          
        [HttpGet("SearchByDonor")]
        public async  Task<ActionResult<Present>> SearchByDonor(string donor)
        {
            return donor == null ? NotFound() : Ok(await presentService.SearchByDonor(donor));
        }

        [HttpGet("SearchByNumOfPurcheses")]
        public async Task<ActionResult<Present>> SearchByNumOfPurchases(int numOfPurcheses)
        {
            return numOfPurcheses == null ? NotFound() : Ok(await presentService.SearchByNumOfPurchases(numOfPurcheses));
        }
    }
}
