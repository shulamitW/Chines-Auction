using Chines_auction_project.DAL;
using Chines_auction_project.Modells;

namespace Chines_auction_project.BLL
{
        public class CategoryService:ICategoryService
        {
            private readonly ICategoryDal categoryDal;
        public CategoryService(ICategoryDal categoryDal) 
        {
            this.categoryDal = categoryDal;
        }
        public async Task<List<Category>> GetCategory()
        {
            return await categoryDal.GetCategory();
        }
        public async Task<Category> AddCategory(Category category)
        {
            return await categoryDal.AddCategory(category);
        }
        public async Task<Category> RemoveCategory(int id)
        {
            return await categoryDal.RemoveCategory(id);
        }
        public async Task<Category> UpdateCategory(Category category, int id)
        {
            return await categoryDal.UpdateCategory(category, id);
        }
    }
}
