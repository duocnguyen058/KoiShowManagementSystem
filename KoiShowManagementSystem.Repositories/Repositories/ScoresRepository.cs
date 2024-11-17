namespace KoiShowManagementSystem.Repositories.Repositories
{
    public interface IScoresRepository : IRepository<Scores>
    {
        List<Scores> GetScoresByEvent(int eventId);
        Scores GetScoreByEventKoiAndJudge(int eventKoiId, int judgeId);
    }
    public class ScoresRepository : RepositoryBase<Scores>, IScoresRepository
    {
        public ScoresRepository(ApplicationDbContext dbContext) : base(dbContext) { }
        public List<Scores> GetScoresByEvent(int eventId)
        {
            return _dbContext.Scores
                .Include(s => s.Event_Koi_Participations)
                .Where(s => s.Event_Koi_Participations.EventsId == eventId)
                .ToList();
        }
        public Scores GetScoreByEventKoiAndJudge(int eventKoiId, int judgeId)
        {
            return _dbContext.Scores
                .FirstOrDefault(s => s.Event_Koi_ParticipationId == eventKoiId && s.UsersId == judgeId);
        }
    }
}
