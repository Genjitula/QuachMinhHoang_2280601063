//using Microsoft.AspNetCore.Mvc;
//using QuachMinhHoang_2280601063.Models;
//using QuachMinhHoang_2280601063.Repositories;
//using System.Threading.Tasks;

//namespace QuachMinhHoang_2280601063.Controllers
//{
//    public class CategoryController : Controller
//    {
//        private readonly ICategoryRepository _categoryRepository;

//        public CategoryController(ICategoryRepository categoryRepository)
//        {
//            _categoryRepository = categoryRepository;
//        }

//        // Hiển thị danh sách các category
//        public async Task<IActionResult> Index()
//        {
//            var categories = await _categoryRepository.GetAllAsync();
//            return View(categories);
//        }

//        // Hiển thị form thêm mới category
//        public IActionResult Add()
//        {
//            return View();
//        }

//        // Xử lý thêm mới category
//        [HttpPost]
//        public async Task<IActionResult> Add(Category category)
//        {
//            if (ModelState.IsValid)
//            {
//                await _categoryRepository.AddAsync(category);
//                return RedirectToAction(nameof(Index));
//            }
//            return View(category);
//        }

//        // Hiển thị form cập nhật category
//        public async Task<IActionResult> Update(int id)
//        {
//            var category = await _categoryRepository.GetByIdAsync(id);
//            if (category == null)
//            {
//                return NotFound();
//            }
//            return View(category);
//        }

//        // Xử lý cập nhật category
//        [HttpPost]
//        public async Task<IActionResult> Update(int id, Category category)
//        {
//            if (id != category.Id)
//            {
//                return NotFound();
//            }

//            if (ModelState.IsValid)
//            {
//                await _categoryRepository.UpdateAsync(category);
//                return RedirectToAction(nameof(Index));
//            }
//            return View(category);
//        }

//        // Hiển thị form xác nhận xóa category
//        public async Task<IActionResult> Delete(int id)
//        {
//            var category = await _categoryRepository.GetByIdAsync(id);
//            if (category == null)
//            {
//                return NotFound();
//            }
//            return View(category);
//        }

//        // Xử lý xóa category
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public async Task<IActionResult> DeleteConfirm(int id)
//        {
//            try
//            {
//                var category = await _categoryRepository.GetByIdAsync(id);
//                if (category == null)
//                {
//                    return NotFound();
//                }

//                await _categoryRepository.DeleteAsync(id);
//                return RedirectToAction(nameof(Index));
//            }
//            catch (Exception ex)
//            {
//                // Log lỗi
//                Console.WriteLine($"Lỗi khi xóa category: {ex.Message}");
//                ModelState.AddModelError("", "Không thể xóa category. Vui lòng thử lại.");
//                return View("Delete", await _categoryRepository.GetByIdAsync(id)); // Hiển thị lại trang xác nhận xóa
//            }
//        }
//    }
//}