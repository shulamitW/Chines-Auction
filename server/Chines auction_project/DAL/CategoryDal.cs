using Chines_auction_project.Modells;
using Microsoft.EntityFrameworkCore;

namespace Chines_auction_project.DAL
{
    public class CategoryDal:ICategoryDal
    {
        private readonly AuctionContex auctionContex;
        public CategoryDal(AuctionContex auctionContex)
        {
            this.auctionContex = auctionContex;
        }
        public async Task<List<Category>> GetCategory()
        {
            try
            {
                return await auctionContex.Category.ToListAsync();

            }
            catch (Exception)
            {

                throw;
            }
        }
        public async Task<Category> AddCategory(Category category)
        {
            try
            {
                await auctionContex.AddAsync(category);
                await auctionContex.SaveChangesAsync();
                return category;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public async Task<Category> RemoveCategory(int id)
        {
            try
            {
                var c = await auctionContex.Category.FirstOrDefaultAsync(c => c.Id == id);
                if (c == null)
                {
                    throw new Exception($"category {id} not found");
                }
                auctionContex.Category.Remove(c);
                await auctionContex.SaveChangesAsync();
                return c;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<Category> UpdateCategory(Category category, int id)
        {
            category.Id = id;
            var c = await auctionContex.Category.FirstOrDefaultAsync(c => c.Id == id);
            if (c == null)
            {
                throw new Exception($"category {id} not found");
            }
            if( category.Description != null ) c.Description= category.Description;
            await auctionContex.SaveChangesAsync();
            return c;
        }
    }
}
