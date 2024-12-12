using AutoMapper;
using Chines_auction_project.BLL;
using Chines_auction_project.Modells;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace Chines_auction_project.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RaffleController : Controller
    {
        private readonly IRaffleService raffleService;
        private readonly IMapper mapper;

        public RaffleController(IRaffleService raffleService, IMapper mapper)
        {
            this.raffleService = raffleService;
            this.mapper = mapper;
        }
        [Authorize(Roles = "Manager")]
        [HttpGet("raffle")]
        public async Task<ActionResult<Present>> raffle()
        {
            var c = await raffleService.raffle();
            return c == null ? NotFound() : Ok(c);
        }
        [Authorize(Roles = "Manager")]
        //[AllowAnonymous]
        [HttpGet("PresentRaffle")]
        public async Task<ActionResult<Present>> PresentRaffle(int presentId)
        {
            var c = await raffleService.PresentRaffle(presentId);
            return c == null ? NotFound() : Ok(c);
        }
        [AllowAnonymous]
        [HttpGet("getAllWinners")]
        public async Task<ActionResult<Present>> getAllWinners()
        {
            var c = await raffleService.getAllWinners();
            return c == null ? NotFound() : Ok(c);
        }
        [Authorize(Roles = "Manager")]
        [HttpGet("SendEmailWithAttachment")]
        public async Task SendEmailWithAttachment(string recipientEmail, string fileName, string filePath)
        {
            await raffleService.SendEmailWithAttachment(recipientEmail, fileName, filePath);
            //return c == null ? NotFound() : Ok(c);
        }
        [Authorize(Roles = "Manager")]
        [HttpGet("ExportToExcelAndSendEmail")]
        public async Task<ActionResult<string>> ExportToExcelAndSendEmail<T>(List<T> dataList, string recipientEmail)
        {
            var c = await raffleService.ExportToExcelAndSendEmail(dataList, recipientEmail);
            return c == null ? NotFound() : Ok(c);
        }
        [Authorize(Roles = "Manager")]
        [HttpGet("ReportOfIncome")]
        public async Task<ActionResult<GiftIncomeReport>> ReportOfIncome()
        {
            var c = await raffleService.ReportOfIncome();
            return c == null ? NotFound() : Ok(c);
        }
        [Authorize(Roles = "Manager")]
        [HttpGet("ReportOfWinners")]
        public async Task<ActionResult<Present>> ReportOfWinners()
        {
            var c = await raffleService.ReportOfWinners();
            return c == null ? NotFound() : Ok(c);
        }
        [Authorize(Roles = "Manager")]
        [HttpGet("SendEmail")]
        public async Task SendEmail(string recipientEmail, string gift)
        {
            await raffleService.SendEmail(recipientEmail, gift);
        }
    }
}


   