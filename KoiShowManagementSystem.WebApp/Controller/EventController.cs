namespace KoiShowManagementSystem.Controllers
{
    public class EventController : Controller
    {
        private readonly IEventsService _eventsService;
        public EventController(IEventsService eventsService)
        {
            _eventsService = eventsService;
        }

        // GET: EventController
        public ActionResult Index()
        {
            var events = _eventsService.GetAllEvents();
            return View(events);
        }

        // GET: EventController/Details/5
        public ActionResult Details(int id)
        {
            var eventDetail = _eventsService.GetEventById(id);
            if (eventDetail == null)
            {
                return NotFound();
            }

            bool isAuthenticated = User.Identity.IsAuthenticated;
            int? userId = isAuthenticated ? int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value) : (int?)null;

            bool canRegister = _eventsService.CanRegisterToEvent(id);
            bool isRegistered = isAuthenticated && _eventsService.IsUserRegisteredToEvent(id, userId.Value);

            var koiParticipationList = _eventsService.GetEventKoiParticipations(id);
            var rankedKoiList = koiParticipationList
                .Where(p => new[] { "Grand Champion", "Mature Champion", "Sakuru Champion" }.Contains(p.Category))
                .OrderBy(p => p.Category)
                .Select(p => new RankedKoiViewModel
                {
                    KoiName = p.Kois.Name,
                    Category = p.Category,
                    Score = p.Score
                }).ToList();

            // Lấy danh sách cá Koi của người dùng (nếu đăng nhập)
            var userKoiList = isAuthenticated ? _eventsService.GetUserKoi(userId.Value) : new List<Koi>();
            ViewBag.UserKoiList = userKoiList;

            var viewModel = new EventDetailsViewModel
            {
                EventDetail = eventDetail,
                CanRegister = isAuthenticated && canRegister && !isRegistered,
                IsRegistered = isRegistered,
                RankedKoiList = rankedKoiList
            };
            return View(viewModel);
        }

        [HttpGet]
        public IActionResult Register(int eventId)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index"); // Chuyển hướng nếu người dùng chưa đăng nhập
            }

            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var userKoiList = _eventsService.GetUserKoi(userId);

            ViewBag.EventId = eventId;
            return View(userKoiList);
        }

        [HttpPost]
        public IActionResult Register(int eventId, int koiId)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index"); // Chuyển hướng nếu người dùng chưa đăng nhập
            }

            _eventsService.RegisterKoiToEvent(eventId, koiId);
            return RedirectToAction("Details", new { id = eventId });
        }

        // GET: EventController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: EventController/Create
        [HttpPost]
        public ActionResult Create(Events eventModel)
        {
            if (eventModel.StartDate < DateTime.Now.Date)
            {
                ModelState.AddModelError("StartDate", "Ngày bắt đầu phải lớn hơn hoặc bằng ngày hiện tại.");
            }
            if (eventModel.EndDate <= eventModel.StartDate)
            {
                ModelState.AddModelError("EndDate", "Ngày kết thúc phải lớn hơn ngày bắt đầu.");
            }
            if (ModelState.IsValid)
            {
                eventModel.Status = DetermineStatus(eventModel.StartDate, eventModel.EndDate);
                _eventsService.CreateEvent(eventModel);
                return RedirectToAction("Index");
            }
            return View(eventModel);
        }

        // GET: EventController/Edit/5
        public ActionResult Edit(int id)
        {
            var eventModel = _eventsService.GetEventById(id);
            if (eventModel == null)
            {
                return NotFound();
            }
            return View(eventModel);
        }

        // POST: EventController/Edit/5
        [HttpPost]
        public ActionResult Edit(Events eventModel)
        {
            if (eventModel.StartDate < DateTime.Now.Date)
            {
                ModelState.AddModelError("StartDate", "Ngày bắt đầu phải lớn hơn hoặc bằng ngày hiện tại.");
            }
            if (eventModel.EndDate <= eventModel.StartDate)
            {
                ModelState.AddModelError("EndDate", "Ngày kết thúc phải lớn hơn ngày bắt đầu.");
            }
            if (ModelState.IsValid)
            {
                eventModel.Status = DetermineStatus(eventModel.StartDate, eventModel.EndDate);
                _eventsService.UpdateEvent(eventModel);
                return RedirectToAction("Index");
            }
            return View(eventModel);
        }

        // POST: EventController/Delete/5
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                var eventToDelete = _eventsService.GetEventById(id);
                if (eventToDelete == null)
                {
                    return NotFound();
                }

                _eventsService.DeleteEvent(id);
                TempData["Message"] = "Sự kiện đã được xóa thành công!";
                TempData["MessageType"] = "success";
            }
            catch (Exception ex)
            {
                TempData["Message"] = "Sự kiện này không thể xóa, vì có ràng buộc dữ liệu liên quan.";
                TempData["MessageType"] = "error";
            }

            return RedirectToAction("Index");
        }

        private string DetermineStatus(DateTime startDate, DateTime endDate)
        {
            var today = DateTime.Now.Date;

            if (today < startDate)
            {
                return "Chưa bắt đầu";
            }
            else if (today >= startDate && today <= endDate)
            {
                return "Đang diễn ra";
            }
            else
            {
                return "Đã kết thúc";
            }
        }

        public ActionResult Search(string keyword)
        {
            var events = _eventsService.SearchEvents(keyword);
            return View("Index", events);
        }
    }
}
