using Chines_auction_project.DAL;
using Chines_auction_project.Modells;

namespace Chines_auction_project.BLL
{
    public class PurchaseService: IPurchaseService
    {
        private readonly IPurchaseDal _purchaseDal;
        public PurchaseService(IPurchaseDal _purchaseDal)
        {
            this._purchaseDal = _purchaseDal;
        }

        public async Task<List<Purchase>> GetAllPurchase()
        {
            return await _purchaseDal.GetAllPurchase();
        }
        //public Purchase GetPurchaseByPresent(Present present);
        public async Task<List<Purchase>> GetAllPurchaseById(int userId)
        {
            return await _purchaseDal.GetAllPurchaseById(userId);
        }
        public async Task<Purchase> GetShoppingCartById(int userId)
        {
            return await _purchaseDal.GetShoppingCartById(userId);
        }
        public async Task<Purchase> Buy(int cartId)//change thr status and pay
        {
            return await _purchaseDal.Buy(cartId);
        }
        public async Task<Purchase> AddToCart(int cartId, int presentId, int userId)//add to cart
        {
            return await _purchaseDal.AddToCart(cartId, presentId, userId);    
        }
        public async Task<List<Purchase>> GetPurchaseByPresent(int presentId)
        {
            return await _purchaseDal.GetPurchaseByPresent(presentId);
        }
        public async Task<Purchase> RemoveFromCart(int cartId, int presentId, int userId)
        {
            return await _purchaseDal.RemoveFromCart(cartId, presentId, userId);
        }

        public async Task<List<Present>> SortByNumOfPurchases()
        {
            return await _purchaseDal.SortByNumOfPurchases();
        }

        //Task<List<Purchase> GetPurchaseByPresent(Present present);
        //Task<Purchase> AddNullPurchase(Purchase purchase);


    }
}
