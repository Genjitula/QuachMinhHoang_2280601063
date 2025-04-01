using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuachMinhHoang_2280601063.Extensions;
using QuachMinhHoang_2280601063.Models;
using QuachMinhHoang_2280601063.Repositories;

namespace QuachMinhHoang_2280601063.Controllers
{
    [Authorize]
    public class ShoppingCartController : Controller
    {
        private readonly IProductRepository _productRepository;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public ShoppingCartController(ApplicationDbContext context,
                                    UserManager<ApplicationUser> userManager,
                                    IProductRepository productRepository)
        {
            _productRepository = productRepository;
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var cart = await GetOrCreateUserCartAsync();
            return View(cart);
        }

        public async Task<IActionResult> AddToCart(int productId, int quantity)
        {
            // Kiểm tra số lượng
            if (quantity <= 0)
            {
                TempData["Error"] = "Số lượng phải lớn hơn 0";
                return RedirectToAction("Index");
            }

            // Lấy thông tin sản phẩm
            var product = await _productRepository.GetByIdAsync(productId);
            if (product == null)
            {
                TempData["Error"] = "Sản phẩm không tồn tại";
                return RedirectToAction("Index");
            }

            // Lấy hoặc tạo giỏ hàng
            var cart = await GetOrCreateUserCartAsync();

            // Kiểm tra và cập nhật giỏ hàng
            var existingItem = cart.Items.FirstOrDefault(i => i.ProductId == productId);
            if (existingItem != null)
            {
                existingItem.Quantity += quantity;
            }
            else
            {
                cart.Items.Add(new CartItem
                {
                    ProductId = productId,
                    Name = product.Name,
                    Price = product.Price,
                    Quantity = quantity,
                    ImageUrl = product.ImageUrl,
                    Product = product
                });
            }

            cart.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();

            TempData["Success"] = $"Đã thêm {product.Name} vào giỏ hàng";
            return RedirectToAction("Index");
        }
        private async Task<Cart> GetOrCreateUserCartAsync()
        {
            var userId = _userManager.GetUserId(User);
            if (string.IsNullOrEmpty(userId))
            {
                throw new UnauthorizedAccessException("User không tồn tại");
            }

            // Tìm giỏ hàng của user hiện tại
            var cart = await _context.Carts
                .Include(c => c.Items)
                    .ThenInclude(i => i.Product)
                .FirstOrDefaultAsync(c => c.UserId == userId);

            // Nếu chưa có giỏ hàng, tạo mới
            if (cart == null)
            {
                cart = new Cart
                {
                    UserId = userId,
                    CreatedAt = DateTime.UtcNow
                };
                _context.Carts.Add(cart);
                await _context.SaveChangesAsync();
            }

            return cart;
        }
        private async Task<Product> GetProductFromDatabase(int productId)
        {
            return await _productRepository.GetByIdAsync(productId);
        }

        public async Task<IActionResult> RemoveFromCart(int productId)
        {
            var cart = await GetOrCreateCartAsync();
            var item = cart.Items.FirstOrDefault(i => i.ProductId == productId);

            if (item == null)
            {
                TempData["Error"] = "Không tìm thấy sản phẩm trong giỏ hàng";
                return RedirectToAction("Index");
            }

            _context.CartItems.Remove(item);
            cart.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();

            TempData["Success"] = $"Đã xóa {item.Product?.Name} khỏi giỏ hàng";
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> OrderCompleted(int? orderId)
        {
            if (orderId == null)
            {
                TempData["Error"] = "Không tìm thấy thông tin đơn hàng";
                return RedirectToAction("Index");
            }

            var order = await _context.Orders
                .Include(o => o.OrderDetails)
                    .ThenInclude(od => od.Product)
                .FirstOrDefaultAsync(o => o.Id == orderId);

            if (order == null || order.UserId != _userManager.GetUserId(User))
            {
                TempData["Error"] = "Bạn không có quyền xem đơn hàng này";
                return RedirectToAction("Index");
            }

            return View(order);
        }

        [HttpGet]
        public async Task<IActionResult> Checkout()
        {
            var cart = await GetOrCreateCartAsync();

            if (cart == null || !cart.Items.Any())
            {
                return RedirectToAction("Index");
            }

            return View(new Order());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Checkout(Order order)
        {
            var cart = await GetOrCreateUserCartAsync();
            if (cart == null || !cart.Items.Any())
            {
                return RedirectToAction("Index");
            }

            if (!ModelState.IsValid)
            {
                return View(order);
            }

            // Tạo đơn hàng mới
            var newOrder = new Order
            {
                UserId = _userManager.GetUserId(User),
                OrderDate = DateTime.UtcNow,
                TotalPrice = cart.GetTotalPrice(),
                ShippingAddress = order.ShippingAddress,
                OrderDetails = cart.Items.Select(i => new OrderDetail
                {
                    ProductId = i.ProductId,
                    Quantity = i.Quantity,
                    Price = i.Price,
                    Product = i.Product
                }).ToList()
            };

            _context.Orders.Add(newOrder);

            // Xóa giỏ hàng sau khi thanh toán
            _context.CartItems.RemoveRange(cart.Items);
            _context.Carts.Remove(cart);

            await _context.SaveChangesAsync();

            return RedirectToAction("OrderCompleted", new { orderId = newOrder.Id });
        }

        private async Task<Cart> GetOrCreateCartAsync()
        {
            var userId = _userManager.GetUserId(User);
            if (string.IsNullOrEmpty(userId))
            {
                throw new InvalidOperationException("User not found");
            }

            var cart = await _context.Carts
                .Include(c => c.Items)
                .ThenInclude(ci => ci.Product)
                .FirstOrDefaultAsync(c => c.UserId == userId);

            if (cart == null)
            {
                cart = new Cart { UserId = userId };
                _context.Carts.Add(cart);
                await _context.SaveChangesAsync();
            }

            return cart;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateCartItem(int productId, int quantity)
        {
            if (quantity < 1)
            {
                TempData["Error"] = "Số lượng không hợp lệ";
                return RedirectToAction("Index");
            }

            var cart = await GetOrCreateCartAsync();
            var item = cart.Items.FirstOrDefault(i => i.ProductId == productId);

            if (item == null)
            {
                TempData["Error"] = "Không tìm thấy sản phẩm trong giỏ hàng";
                return RedirectToAction("Index");
            }

            item.Quantity = quantity;
            cart.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();

            TempData["Success"] = "Đã cập nhật giỏ hàng";
            return RedirectToAction("Index");
        }
    }
}