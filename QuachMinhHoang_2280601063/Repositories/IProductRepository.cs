using QuachMinhHoang_2280601063.Models;

namespace QuachMinhHoang_2280601063.Repositories
{
    public interface IProductRepository
    {
        Task<IEnumerable<Product>> GetAllAsync();
        Task<Product> GetByIdAsync(int id);
        Task AddAsync(Product product);
        Task UpdateAsync(Product product);
        Task DeleteAsync(int id);
        Task<bool> HasProductsInCategory(int categoryId);
        Task<int> GetTotalProductsCount();
    }
}
