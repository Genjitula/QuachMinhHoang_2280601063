using Microsoft.EntityFrameworkCore;
using QuachMinhHoang_2280601063.Models;

namespace QuachMinhHoang_2280601063.Repositories
{
    public class EFProductRepository : IProductRepository
    {
        private readonly ApplicationDbContext _context;
        public EFProductRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            // return await _context.Products.ToListAsync();
            return await _context.Products
            .Include(p => p.Category) // Include thông tin về category
            .ToListAsync();
        }
        public async Task<Product> GetByIdAsync(int id)
        {
            // return await _context.Products.FindAsync(id);
            // lấy thông tin kèm theo category
            return await _context.Products.Include(p =>
           p.Category).FirstOrDefaultAsync(p => p.Id == id);
        }
        public async Task AddAsync(Product product)
        {
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
        }
        public async Task UpdateAsync(Product product)
        {
            _context.Products.Update(product);
            await _context.SaveChangesAsync();
        }
        public async Task DeleteAsync(int id)
        {
            var product = await _context.Products.FindAsync(id);
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
        }
        public async Task<bool> HasProductsInCategory(int categoryId)
        {
            return await _context.Products.AnyAsync(p => p.CategoryId == categoryId);
        }
        public async Task<int> GetTotalProductsCount()
        {
            return await _context.Products.CountAsync();
        }
    }
}
