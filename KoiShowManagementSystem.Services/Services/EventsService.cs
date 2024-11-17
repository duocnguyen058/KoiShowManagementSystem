using Microsoft.EntityFrameworkCore;

namespace KoiShowManagementSystem.Services.Services
{
    public interface IEventsService
    {
        List<Events> GetAllEvents();
        Events GetEventById(int eventId);
        void CreateEvent(Events events);
        void UpdateEvent(Events events);
        void DeleteEvent(int eventId);
        void RegisterKoiToEvent(int eventId, int koiId);
        List<Events> SearchEvents(string keyword);
        List<Koi> GetUserKoi(int userId); // Thêm phương thức để lấy cá Koi của người dùng
        bool CanRegisterToEvent(int eventId); // Phương thức để kiểm tra trạng thái sự kiện
        bool IsUserRegisteredToEvent(int eventId, int userId); // Kiểm tra người dùng đã đăng ký chưa
        List<Event_Koi_Participation> GetParticipantsByEvent(int eventId);
        List<Scores> GetScoresByEvent(int eventId);
        List<JudgeAssignments> GetJudgesByEvent(int eventId);
        IEnumerable<Event_Koi_Participation> GetEventKoiParticipations(int eventId);
    }
    public class EventsService : IEventsService
    {
        private readonly IEventsRepository _eventsRepository;
        private readonly IKoiRepository _koiRepository;
        private readonly IEventKoiParticipationRepository _eventKoiParticipationRepository;
        private readonly IScoresRepository _scoresRepository;
        private readonly IJudgeAssignmentsRepository _judgeAssignmentsRepository;

        public EventsService(
            IEventsRepository eventsRepository,
            IKoiRepository koiRepository,
            IEventKoiParticipationRepository eventKoiParticipationRepository,
            IScoresRepository scoresRepository,
            IJudgeAssignmentsRepository judgeAssignmentsRepository)
        {
            _eventsRepository = eventsRepository;
            _koiRepository = koiRepository;
            _eventKoiParticipationRepository = eventKoiParticipationRepository;
            _scoresRepository = scoresRepository;
            _judgeAssignmentsRepository = judgeAssignmentsRepository;
        }
        public List<Koi> GetUserKoi(int userId)
        {
            return _koiRepository.GetAllKoi().Where(k => k.UsersId == userId).ToList();
        }
        public bool CanRegisterToEvent(int eventId)
        {
            var eventDetails = _eventsRepository.GetById(eventId);
            return eventDetails.Status == "Chưa bắt đầu" || eventDetails.Status == "Đang diễn ra";
        }
        //
        public List<Events> GetAllEvents()
        {
            return _eventsRepository.GetEvents();
        }
        //
        public Events GetEventById(int eventId)
        {
            return _eventsRepository.GetById(eventId);
        }
        //
        public void CreateEvent(Events events)
        {
            _eventsRepository.Add(events);
        }
        //
        public void UpdateEvent(Events events)
        {
            _eventsRepository.Update(events);
        }
        //
        public void DeleteEvent(int eventId)
        {
            var eventToDelete = _eventsRepository.GetById(eventId);
            if (eventToDelete != null)
            {
                _eventsRepository.Delete(eventToDelete);
            }
        }
        //
        public void RegisterKoiToEvent(int eventId, int koiId)
        {
            // Lấy thông tin cá Koi từ ID
            var koi = _koiRepository.GetById(koiId);
            if (koi == null)
            {
                throw new ArgumentException("Cá Koi không tồn tại.");
            }

            // Tự động xác định hạng mục thi đấu dựa trên Size và Age
            string category;
            if (koi.Size >= 25)
            {
                category = "Grand Champion";
            }
            else if (koi.Age >= 5)
            {
                category = "Mature Champion";
            }
            else
            {
                category = "Sakuru Champion";
            }

            // Tạo mới một bản ghi tham gia sự kiện với hạng mục đã xác định
            var participation = new Event_Koi_Participation
            {
                EventsId = eventId,
                KoiId = koiId,
                Category = category,
                Score = 0
            };

            // Lưu vào cơ sở dữ liệu
            _eventKoiParticipationRepository.Add(participation);
        }
        public List<Events> SearchEvents(string keyword)
        {
            return _eventsRepository.GetEvents()
                .Where(e => e.EventName.Contains(keyword) || e.Description.Contains(keyword))
                .ToList();
        }
        public bool IsUserRegisteredToEvent(int eventId, int userId)
        {
            var userKoiList = _koiRepository.GetAllKoi().Where(k => k.UsersId == userId).Select(k => k.Id).ToList();
            return _eventKoiParticipationRepository.GetParticipantsByEvent(eventId)
                                                   .Any(p => userKoiList.Contains(p.KoiId));
        }
        public List<Event_Koi_Participation> GetParticipantsByEvent(int eventId)
        {
            return _eventKoiParticipationRepository.GetParticipantsByEvent(eventId);
        }
        public List<Scores> GetScoresByEvent(int eventId)
        {
            return _scoresRepository.GetScoresByEvent(eventId);
        }
        public List<JudgeAssignments> GetJudgesByEvent(int eventId)
        {
            return _judgeAssignmentsRepository.GetJudgesByEvent(eventId);
        }
        public IEnumerable<Event_Koi_Participation> GetEventKoiParticipations(int eventId)
        {
            return _eventKoiParticipationRepository.GetEventKoiParticipations(eventId);
        }
    }
}
