using AutoMapper;
using Chines_auction_project.BLL;
using Chines_auction_project.Modells;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Authorization;
using System.Data;

namespace Chines_auction_project.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PurchaseController:ControllerBase
    {

        private readonly IMapper mapper;
        private readonly IPurchaseService purchaseService;
        public PurchaseController(IMapper mapper, IPurchaseService purchaseService)
        {
            this.mapper = mapper;
            this.purchaseService = purchaseService;
        }
        [Authorize(Roles = "Manager")]
        [HttpGet("GetAllPurchase")]
        public async Task<ActionResult<Purchase>> GetAllPurchase()
        {
            var p =await purchaseService.GetAllPurchase();
            return p == null ? NotFound() : Ok(p);
        }
        [Authorize(Roles = "Manager")]
        [HttpGet("SortByNumOfPurchases")]
        public async Task<ActionResult<Present>> SortByNumOfPurchases()
        {
            var p = await purchaseService.SortByNumOfPurchases();
            return p == null ? NotFound() : Ok(p);
        }

        //public Purchase GetPurchaseByPresent(Present present);
        [Authorize(Roles = "Manager")]
        [HttpGet("GetPurchaseById/{userId}")]
        public async Task<ActionResult<Purchase>> GetAllPurchaseById(int userId)
        {
            var p = await purchaseService.GetAllPurchaseById(userId);
            return p == null ? NotFound() : Ok(p);
        }

        [Authorize(Roles = "User")]
        [HttpPost("GetShoppingCartById/{userId}")]
        public async Task<ActionResult<Purchase>> GetShoppingCartById(int userId)
        {
            var p = await purchaseService.GetShoppingCartById(userId);
            return p == null ? NotFound() : Ok(p);
        }
        
        [Authorize(Roles = "User")]
        [HttpPost("Buy")]
        public async Task<ActionResult<Purchase>> Buy(int cartId)//change thr status and pay
        {
            //var p = mapper.Map<Purchase>(purchase);
            return cartId == null ? NotFound() : Ok(await purchaseService.Buy(cartId));
        }//change thr status and pay
        
        [Authorize(Roles = "User")]
        [HttpPost("AddToCart")]
        public async Task<ActionResult<Purchase>> AddToCart(int cartId, int presentId, int userId)
        {
            return cartId == 0|| presentId==0 || userId==0 ? NotFound() : Ok(await purchaseService.AddToCart(cartId, presentId, userId));
        }
        [Authorize(Roles = "User")]
        [HttpPost("RemoveFromCart")]

        public async Task<ActionResult<Purchase>> RemoveFromCart(int cartId, int presentId, int userId)
        {
            return cartId == 0 || presentId == 0 || userId == 0 ? NotFound() : Ok(await purchaseService.RemoveFromCart(cartId, presentId, userId));
        }

        [Authorize(Roles = "Manager")]
        [HttpGet("GetPurchaseByPresent")]
        public async Task<ActionResult<Purchase>> GetPurchaseByPresent(int presentId)
        {
            return presentId == 0 ? NotFound() : Ok(await purchaseService.GetPurchaseByPresent(presentId));
        }
    }
}
