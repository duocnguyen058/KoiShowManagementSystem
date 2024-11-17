namespace KoiShowManagementSystem.Controllers
{
    public class ScoringController : Controller
    {
        private readonly IScoresService _scoresService;
        private readonly IUserService _userService;
        private readonly IKoiService _koiService;
        private readonly IEventKoiParticipationService _eventKoiParticipationService; // Service để lấy danh sách cá Koi trong sự kiện
        private readonly IJudgeAssignmentsService _judgeAssignmentsService; // Service để lấy danh sách sự kiện của giám khảo

        public ScoringController(
            IScoresService scoresService,
            IUserService userService,
            IEventKoiParticipationService eventKoiParticipationService,
            IJudgeAssignmentsService judgeAssignmentsService,
            IKoiService koiService)
        {
            _scoresService = scoresService;
            _userService = userService;
            _eventKoiParticipationService = eventKoiParticipationService;
            _judgeAssignmentsService = judgeAssignmentsService;
            _koiService = koiService;
        }

        public async Task<IActionResult> Index()
        {
            var currentUserId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            var judgeAssignments = _judgeAssignmentsService.GetEventsByJudgeId(currentUserId);

            return View(judgeAssignments);
        }

        public IActionResult ScoreEvent(int eventId)
        {
            // Lấy danh sách Event_Koi_Participation theo EventId
            var eventKoiParticipations = _eventKoiParticipationService.GetKoiByEventId(eventId);

            // Lấy danh sách KoiId từ các tham gia
            var koiIds = eventKoiParticipations.Select(ep => ep.KoiId).ToList();

            // Lấy thông tin chi tiết về các cá Koi
            var koiDetails = _koiService.GetAllKoi().Where(k => koiIds.Contains(k.Id)).ToList();

            // Tạo ViewModel
            var viewModel = eventKoiParticipations.Select(ep => new ScoreEventViewModel
            {
                ParticipationId = ep.Id,
                KoiName = koiDetails.FirstOrDefault(k => k.Id == ep.KoiId)?.Name ?? "Không rõ",
                Category = ep.Category,
                CurrentScore = ep.Score,
                PhotoPath = koiDetails.FirstOrDefault(k => k.Id == ep.KoiId)?.PhotoPath ?? "Không có ảnh",
            }).ToList();
            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> SubmitScore(int eventKoiParticipationId, float shapeScore, float colorScore, float patternScore)
        {
            var currentUserId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var currentUser = await _userService.GetUserById(currentUserId);

            if (currentUser == null)
            {
                TempData["Error"] = "Người dùng không tồn tại.";
                return RedirectToAction("Index");
            }

            try
            {
                // Gọi service để thêm điểm mới cho cá Koi
                _scoresService.AddScore(eventKoiParticipationId, currentUser.Id, shapeScore, colorScore, patternScore);
                return Json(new { success = true, message = "Lưu điểm thành công!" });
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
            }

            return RedirectToAction("Index");
        }
    }
}
