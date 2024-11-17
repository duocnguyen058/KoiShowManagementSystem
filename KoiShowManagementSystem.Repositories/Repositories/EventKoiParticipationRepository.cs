using Microsoft.EntityFrameworkCore;

namespace KoiShowManagementSystem.Repositories.Repositories
{
    public interface IEventKoiParticipationRepository : IRepository<Event_Koi_Participation>
    {
        List<Event_Koi_Participation> GetParticipantsByEvent(int eventId);
        IEnumerable<Event_Koi_Participation> GetEventKoiParticipations(int eventId);
    }
    public class EventKoiParticipationRepository : RepositoryBase<Event_Koi_Participation>, IEventKoiParticipationRepository
    {
        public EventKoiParticipationRepository(ApplicationDbContext dbContext) : base(dbContext) { }

        public List<Event_Koi_Participation> GetParticipantsByEvent(int eventId)
        {
            return _dbContext.Event_Koi_Participations.Where(p => p.EventsId == eventId).ToList();
        }
        public IEnumerable<Event_Koi_Participation> GetEventKoiParticipations(int eventId)
        {
            return _dbContext.Event_Koi_Participations
                           .Where(ep => ep.EventsId == eventId)
                           .Include(ep => ep.Kois) // Bao gồm thông tin về Koi
                           .ToList();
        }
    }
}
