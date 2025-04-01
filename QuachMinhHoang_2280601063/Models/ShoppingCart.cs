using Microsoft.AspNetCore.Mvc;

namespace QuachMinhHoang_2280601063.Models
{
    public class ShoppingCart
    {
        public List<CartItem> Items { get; set; } = new List<CartItem>();

        public void AddItem(CartItem item)
        {
            var existingItem = Items.FirstOrDefault(i => i.ProductId == item.ProductId);
            if (existingItem != null)
            {
                existingItem.Quantity += item.Quantity;
            }
            else
            {
                Items.Add(item);
            }
        }
        public void UpdateQuantity(int productId, int quantity)
        {
            var item = Items.FirstOrDefault(i => i.ProductId == productId);
            if (item != null)
            {
                item.Quantity = quantity;
            }
        }
        public bool RemoveItem(int productId)
        {
            var item = Items.FirstOrDefault(i => i.ProductId == productId);
            if (item != null)
            {
                return Items.Remove(item);
            }
            return false;
        }

        // Tính tổng tiền
        public decimal GetTotalPrice()
        {
            return Items.Sum(item => item.Price * item.Quantity);
        }

        // Đếm tổng số sản phẩm (tính cả số lượng từng loại)
        public int GetTotalItemsCount()
        {
            return Items.Sum(item => item.Quantity);
        }

        // Đếm số loại sản phẩm khác nhau
        public int GetDistinctItemsCount()
        {
            return Items.Count;
        }

        // Xóa toàn bộ giỏ hàng
        public void Clear()
        {
            Items.Clear();
        }

        // Kiểm tra giỏ hàng có sản phẩm nào không
        public bool IsEmpty()
        {
            return !Items.Any();
        }

        // Lấy thông tin một sản phẩm trong giỏ hàng
        public CartItem GetItem(int productId)
        {
            return Items.FirstOrDefault(i => i.ProductId == productId);
        }

        // Kiểm tra sản phẩm có trong giỏ hàng chưa
        public bool ContainsProduct(int productId)
        {
            return Items.Any(i => i.ProductId == productId);
        }
    }
}
