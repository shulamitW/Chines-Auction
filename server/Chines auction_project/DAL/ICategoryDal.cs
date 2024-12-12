using Chines_auction_project.Modells;

namespace Chines_auction_project.DAL
{
    public interface ICategoryDal
    {
        Task<List<Category>> GetCategory();
        Task<Category> AddCategory(Category category);
        Task<Category> RemoveCategory(int id);
        Task<Category> UpdateCategory( Category category, int id);
    }
}