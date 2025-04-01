namespace QuachMinhHoang_2280601063.Models
{
    public class CartItem
    {
        public int Id { get; set; }
        public int CartId { get; set; } // Khóa ngoại tới Cart
        public Cart Cart { get; set; }

        public int ProductId { get; set; }
        public Product Product { get; set; }

        // Thông tin lưu trữ (phòng khi sản phẩm thay đổi)
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string ImageUrl { get; set; }

        public int Quantity { get; set; }

        public decimal GetTotalPrice() => Price * Quantity;
    }
}
