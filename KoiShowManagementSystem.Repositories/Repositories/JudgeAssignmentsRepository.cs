namespace KoiShowManagementSystem.Repositories.Repositories
{
    public interface IJudgeAssignmentsRepository : IRepository<JudgeAssignments>
    {
        List<JudgeAssignments> GetJudgesByEvent(int eventId);
    }
    public class JudgeAssignmentsRepository : RepositoryBase<JudgeAssignments>, IJudgeAssignmentsRepository
    {
        public JudgeAssignmentsRepository(ApplicationDbContext dbContext) : base(dbContext) { }

        public List<JudgeAssignments> GetJudgesByEvent(int eventId)
        {
            return _dbContext.JudgeAssignments.Where(j => j.EventsId == eventId).ToList();
        }
    }
}
