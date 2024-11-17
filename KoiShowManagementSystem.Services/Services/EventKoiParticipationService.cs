namespace KoiShowManagementSystem.Services.Services
{
    public interface IEventKoiParticipationService
    {
        void RegisterKoiToEvent(int eventId, int koiId);
        void AssignCategoryToKoi(int eventKoiId, string category);
        List<Event_Koi_Participation> GetParticipantsByEvent(int eventId);
        List<Event_Koi_Participation> GetKoiByEventId(int eventId);
    }

    public class EventKoiParticipationService : IEventKoiParticipationService
    {
        private readonly IEventKoiParticipationRepository _eventKoiParticipationRepository;
        private readonly ApplicationDbContext _dbcontext;

        public EventKoiParticipationService(IEventKoiParticipationRepository eventKoiParticipationRepository, ApplicationDbContext dbcontext)
        {
            _eventKoiParticipationRepository = eventKoiParticipationRepository;
            _dbcontext = dbcontext; 
        }

        public void RegisterKoiToEvent(int eventId, int koiId)
        {
            var participation = new Event_Koi_Participation
            {
                EventsId = eventId,
                KoiId = koiId,
                Category = null // Chưa phân hạng lúc đăng ký
            };
            _eventKoiParticipationRepository.Add(participation);
        }

        public void AssignCategoryToKoi(int eventKoiId, string category)
        {
            var participation = _eventKoiParticipationRepository.GetById(eventKoiId);
            if (participation != null)
            {
                participation.Category = category;
                _eventKoiParticipationRepository.Update(participation);
            }
        }

        public List<Event_Koi_Participation> GetParticipantsByEvent(int eventId)
        {
            return _eventKoiParticipationRepository.GetParticipantsByEvent(eventId);
        }

        public List<Event_Koi_Participation> GetKoiByEventId(int eventId)
        {
            return _dbcontext.Event_Koi_Participations
                           .Where(ep => ep.EventsId == eventId)
                           .ToList();
        }
    }
}
