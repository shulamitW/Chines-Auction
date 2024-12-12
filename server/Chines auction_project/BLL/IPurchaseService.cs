using Chines_auction_project.Modells;

namespace Chines_auction_project.BLL
{
    public interface IPurchaseService
    {
        Task<List<Purchase>> GetAllPurchase();
        //public Purchase GetPurchaseByPresent(Present present);
        Task<List<Purchase>> GetAllPurchaseById(int userId);
        Task<Purchase> GetShoppingCartById(int userId);
        Task<Purchase> Buy(int cartId);//change thr status and pay

        Task<Purchase> AddToCart(int cartId, int presentId, int userId);//add to cart
        Task<List<Purchase>> GetPurchaseByPresent(int presentId);

        Task<Purchase> RemoveFromCart(int cartId, int presentId, int userId);//remove from cart
        Task<List<Present>> SortByNumOfPurchases();


        //Task<Purchase> AddNullPurchase(Purchase purchase);
    }
}