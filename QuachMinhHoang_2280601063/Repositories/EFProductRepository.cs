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
            return await _context.Products
                .Include(p => p.Category)
                .ToListAsync();
        }
        public async Task<Product> GetByIdAsync(int id)
        {
            return await _context.Products.FindAsync(id);
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
            var product = await _context.Products
                .Include(p => p.Images) // Nếu có bảng liên quan (ví dụ: ProductImage)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (product != null)
            {
                // Xóa các bản ghi liên quan (nếu có)
                if (product.Images != null && product.Images.Any())
                {
                    _context.ProductImages.RemoveRange(product.Images);
                }

                // Xóa sản phẩm
                _context.Products.Remove(product);
                await _context.SaveChangesAsync();
            }
        }
    }
}
