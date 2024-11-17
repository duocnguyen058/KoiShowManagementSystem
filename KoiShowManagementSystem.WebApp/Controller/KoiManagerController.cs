using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using KoiShowManagementSystem.Services;
using KoiShowManagementSystem.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace KoiShowManagementSystem.Controllers
{
    public class KoiManagerController : Controller
    {
        private readonly IKoiService _koiService;
        private readonly ILogger<KoiManagerController> _logger;

        public KoiManagerController(IKoiService koiService, ILogger<KoiManagerController> logger)
        {
            this._koiService = koiService;
            this._logger = logger;
        }

        // GET: KoiManagerController
        public IActionResult Index(string search = "")
        {
            var userId = GetUserId();
            if (userId == null) return Unauthorized();

            var koiList = _koiService.GetKoiByUserId(userId.Value);

            if (!string.IsNullOrEmpty(search))
            {
                koiList = koiList.Where(k => k.Name.Contains(search, StringComparison.OrdinalIgnoreCase) ||
                                             k.Variety.Contains(search, StringComparison.OrdinalIgnoreCase)).ToList();
            }

            if (!koiList.Any())
            {
                ViewBag.Message = "Chưa có hồ sơ cá Koi nào. Bạn có thể thêm cá Koi mới.";
            }

            return View(koiList);
        }

        // GET: KoiManagerController/IndexMng
        [HttpGet]
        public IActionResult IndexMng(string search = "")
        {
            if (!User.IsInRole("ADMIN"))
            {
                return Unauthorized();
            }

            var koiList = _koiService.GetAllKoi();

            if (!string.IsNullOrEmpty(search))
            {
                koiList = koiList.Where(k => k.Name.Contains(search, StringComparison.OrdinalIgnoreCase) ||
                                             k.Variety.Contains(search, StringComparison.OrdinalIgnoreCase)).ToList();
            }

            return View(koiList);
        }

        // GET: KoiManagerController/Details/5
        public ActionResult Details(int id)
        {
            var koi = _koiService.GetKoiById(id);
            if (koi == null || !CanAccessKoi(koi))
            {
                return NotFound();
            }

            return View(koi);
        }

        // GET: KoiManagerController/Create
        public ActionResult Create() => View();

        // POST: KoiManagerController/Create
        [HttpPost]
        public async Task<IActionResult> Create(Koi koi, IFormFile? photo)
        {
            var userId = GetUserId();
            if (userId == null) return Unauthorized();

            koi.UsersId = userId.Value;

            if (photo != null && !IsValidPhoto(photo))
            {
                return View(koi);
            }

            if (photo != null)
            {
                koi.PhotoPath = await SavePhotoAsync(photo);
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _koiService.CreateKoi(koi);
                    TempData["SuccessMessage"] = "Cá Koi đã được thêm thành công!";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Error while creating Koi: {ex.Message}");
                    ModelState.AddModelError("", "Có lỗi xảy ra khi tạo cá Koi. Vui lòng thử lại.");
                }
            }

            return View(koi);
        }

        // GET: KoiManagerController/Edit/5
        public IActionResult Edit(int id)
        {
            var koi = _koiService.GetKoiById(id);
            if (koi == null || koi.UsersId != GetUserId()) return NotFound();

            ViewBag.RegistrationStatuses = GetRegistrationStatuses();

            return View(koi);
        }

        // POST: KoiManagerController/Edit/5
        [HttpPost]
        public async Task<IActionResult> Edit(int id, Koi koi, IFormFile? photo)
        {
            var currentUserId = GetUserId();
            if (currentUserId == null) return Unauthorized();

            var existingKoi = _koiService.GetKoiById(id);
            if (existingKoi == null || existingKoi.UsersId != currentUserId) return Unauthorized();

            existingKoi.Name = koi.Name;
            existingKoi.Variety = koi.Variety;
            existingKoi.Size = koi.Size;
            existingKoi.Age = koi.Age;
            existingKoi.RegistrationStatus = koi.RegistrationStatus;

            if (photo != null && !IsValidPhoto(photo))
            {
                return View(koi);
            }

            if (photo != null)
            {
                existingKoi.PhotoPath = await SavePhotoAsync(photo);
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _koiService.UpdateKoi(existingKoi);
                    TempData["SuccessMessage"] = "Cá Koi đã được cập nhật thành công!";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Error while updating Koi: {ex.Message}");
                    ModelState.AddModelError("", "Có lỗi xảy ra khi cập nhật cá Koi. Vui lòng thử lại.");
                }
            }

            ViewBag.RegistrationStatuses = GetRegistrationStatuses();

            return View(koi);
        }

        // POST: KoiManagerController/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var koi = _koiService.GetKoiById(id);
            if (koi == null || koi.UsersId != GetUserId()) return NotFound();

            try
            {
                _koiService.DeleteKoi(id);
                TempData["SuccessMessage"] = "Cá Koi đã được xóa thành công!";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error while deleting Koi: {ex.Message}");
                TempData["ErrorMessage"] = "Có lỗi xảy ra khi xóa cá Koi. Vui lòng thử lại.";
                return RedirectToAction(nameof(Index));
            }
        }

        // Helper method to validate photo file
        private bool IsValidPhoto(IFormFile photo)
        {
            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png" };
            var fileExtension = Path.GetExtension(photo.FileName).ToLower();

            if (!allowedExtensions.Contains(fileExtension))
            {
                ModelState.AddModelError("photo", "Chỉ cho phép tải lên các định dạng ảnh: .jpg, .jpeg, .png");
                return false;
            }

            if (photo.Length > 5 * 1024 * 1024)  // 5 MB max size
            {
                ModelState.AddModelError("photo", "Kích thước ảnh không được vượt quá 5 MB");
                return false;
            }

            return true;
        }

        // Helper method to save photo and return the file path
        private async Task<string> SavePhotoAsync(IFormFile photo)
        {
            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
            Directory.CreateDirectory(uploadsFolder); // Ensure the directory exists
            var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(photo.FileName);
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await photo.CopyToAsync(stream);
            }
            return "/uploads/" + uniqueFileName; // Return the photo's relative URL
        }

        // Helper method to get current user ID
        private int? GetUserId()
        {
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return string.IsNullOrEmpty(userIdClaim) ? (int?)null : int.Parse(userIdClaim);
        }

        // Helper method to check if the user can access the koi
        private bool CanAccessKoi(Koi koi)
        {
            var userId = GetUserId();
            return userId != null && (User.IsInRole("ADMIN") || koi.UsersId == userId.Value);
        }

        // Helper method to return registration status options
        private List<SelectListItem> GetRegistrationStatuses()
        {
            return new List<SelectListItem>
            {
                new SelectListItem { Value = "Hoạt động", Text = "Hoạt động" },
                new SelectListItem { Value = "Tạm ngừng", Text = "Tạm ngừng" },
                new SelectListItem { Value = "Ngừng hoạt động", Text = "Ngừng hoạt động" }
            };
        }
    }
}
