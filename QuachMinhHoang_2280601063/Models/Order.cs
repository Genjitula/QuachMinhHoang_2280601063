using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using QuachMinhHoang_2280601063.Migrations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuachMinhHoang_2280601063.Models
{
    public class Order
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalPrice { get; set; }
        public string ShippingAddress { get; set; }
        public string Notes { get; set; }

        [ForeignKey("UserId")]
        [ValidateNever]
        public ApplicationUser ApplicationUser { get; set; }
        public List<OrderDetail> OrderDetails { get; set; }

    }
}
