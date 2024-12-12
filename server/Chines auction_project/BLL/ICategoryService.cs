using Chines_auction_project.Modells;

namespace Chines_auction_project.BLL
{
    public interface ICategoryService
    {
        Task<List<Category>> GetCategory();
        Task<Category> AddCategory(Category category);
        Task<Category> RemoveCategory(int id);
        Task<Category> UpdateCategory(Category category, int id);
    }
}