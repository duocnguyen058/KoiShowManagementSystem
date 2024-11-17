using KoiShowManagementSystem.Services.Services;
using KoiShowManagementSystem.ViewModels;
using Microsoft.AspNetCore.Mvc;


         
     
       namespace KoiShowManagementSystem.Controllers
{
    public class JudgeAssignmentsController : Controller
    {
        private readonly IUserService _userService;
        private readonly IJudgeAssignmentsService _judgeAssignmentsService;
        private readonly IEventsService _eventsService;
        private readonly ILogger<JudgeAssignmentsController> _logger;

        public JudgeAssignmentsController(IUserService userService, IJudgeAssignmentsService judgeAssignmentsService, IEventsService eventsService, ILogger<JudgeAssignmentsController> logger)
        {
            _userService = userService;
            _judgeAssignmentsService = judgeAssignmentsService;
            _eventsService = eventsService;
            _logger = logger;
        }

        // GET: /JudgeAssignments/
        public IActionResult Index()
        {
            var referees = _userService.GetUsersByRole("REFEREE");

            var refereeAssignments = referees.Select(referee => new RefereeViewModel
            {
                UserId = referee.Id,
                UserName = referee.Username,
                AssignedEventCount = _judgeAssignmentsService.GetEventsByJudge(referee.Id).Count()
            }).ToList();

            return View(refereeAssignments);
        }

        // GET: /JudgeAssignments/AssignJudge
        [Route("JudgeAssignments/AssignJudge")]
        public IActionResult AssignJudge(int userId)
        {
            try
            {
                var user = _userService.GetUserById(userId);
                if (user == null)
                {
                    return NotFound("Giám khảo không tồn tại");
                }

                var upcomingEvents = _eventsService.GetAllEvents()
                    .Where(e => e.Status == "Chưa bắt đầu" || e.Status == "Đang diễn ra")
                    .ToList();

                if (!upcomingEvents.Any())
                {
                    return NotFound("Không có sự kiện nào sắp diễn ra hoặc đang diễn ra.");
                }

                var assignViewModel = new AssignJudgeViewModel
                {
                    UserId = userId,
                    Events = upcomingEvents,
                };

                return PartialView("_AssignJudgeModal", assignViewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading assign judge modal for user {UserId}.", userId);
                return StatusCode(500, "Đã xảy ra lỗi khi tải modal phân công.");
            }
        }

        // POST: /JudgeAssignments/AssignJudge
        [HttpPost]
        [Route("JudgeAssignments/AssignJudge")]
        public IActionResult AssignJudge(AssignJudgeViewModel model, string action)
        {
            if (model.SelectedEventIds != null && model.SelectedEventIds.Any())
            {
                foreach (var eventId in model.SelectedEventIds)
                {
                    string message = string.Empty;

                    if (action == "Assign")
                    {
                        message = _judgeAssignmentsService.AssignJudgeToEvent(eventId, model.UserId);
                    }
                    else if (action == "Unassign")
                    {
                        message = _judgeAssignmentsService.RemoveJudgeFromEvent(eventId, model.UserId);
                    }

                    if (message.Contains("Giám khảo đã được phân công") || message.Contains("Giám khảo chưa được phân công"))
                    {
                        TempData["ErrorMessage"] = message; // Lưu thông báo vào TempData
                        return RedirectToAction("Index"); // Quay lại trang hiện tại
                    }
                }
            }

            TempData["SuccessMessage"] = (action == "Assign") ? "Phân công giám khảo thành công." : "Hủy phân công giám khảo thành công.";
            return RedirectToAction("Index");
        }
    }
}




