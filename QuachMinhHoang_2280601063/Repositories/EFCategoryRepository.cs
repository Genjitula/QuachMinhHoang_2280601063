using Microsoft.EntityFrameworkCore;
using QuachMinhHoang_2280601063.Models;

namespace QuachMinhHoang_2280601063.Repositories
{
    public class EFCategoryRepository : ICategoryRepository
    {
        private readonly ApplicationDbContext _context;
        public EFCategoryRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Category>> GetAllAsync()
        {
            return await _context.Categories.ToListAsync();
        }
        public async Task<Category> GetByIdAsync(int id)
        {
            return await _context.Categories.FindAsync(id);
        }
        public async Task AddAsync(Category category)
        {
            _context.Categories.Add(category);
            await _context.SaveChangesAsync();
        }
        public async Task UpdateAsync(Category category)
        {
            _context.Categories.Update(category);
            await _context.SaveChangesAsync();
        }
        public async Task DeleteAsync(int id)
        {
            var category = await _context.Categories
                .Include(c => c.Products) // Nếu có bảng liên quan (ví dụ: Products)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (category != null)
            {
                // Xóa các bản ghi liên quan (nếu có)
                if (category.Products != null && category.Products.Any())
                {
                    _context.Products.RemoveRange(category.Products);
                }

                // Xóa category
                _context.Categories.Remove(category);
                await _context.SaveChangesAsync();
            }
        }
    }
}
