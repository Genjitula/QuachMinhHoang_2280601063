namespace QuachMinhHoang_2280601063.Models
{
    public class Cart
    {
        public int Id { get; set; }
        public string UserId { get; set; } // Liên kết với User
        public List<CartItem> Items { get; set; } = new List<CartItem>();
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }

        // Tính tổng tiền
        public decimal GetTotalPrice() => Items.Sum(item => item.Price * item.Quantity);

        // Các phương thức tiện ích
        public void Clear()
        {
            Items.Clear();
            UpdatedAt = DateTime.UtcNow;
        }

        public bool IsEmpty() => !Items.Any();
    }
}
