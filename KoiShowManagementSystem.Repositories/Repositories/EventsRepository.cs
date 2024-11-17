namespace KoiShowManagementSystem.Repositories.Repositories
{
    public interface IEventsRepository : IRepository<Events>
    {
        List<Events> GetEvents();
    }
    public class EventsRepository : RepositoryBase<Events>, IEventsRepository
    {
        public EventsRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }

        public List<Events> GetEvents()
        {
            return _dbContext.Events.ToList();
        }
    }
}
