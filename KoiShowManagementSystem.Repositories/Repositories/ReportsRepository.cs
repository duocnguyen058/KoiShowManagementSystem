namespace KoiShowManagementSystem.Repositories.Repositories
{
    public interface IReportsRepository : IRepository<Reports>
    {
        List<Reports> GetReportsByEvent(int eventId);
        Reports GetPredictionReport(int eventId);
        Reports GetStatisticsReport(int eventId);
    }

    public class ReportsRepository : RepositoryBase<Reports>, IReportsRepository
    {
        public ReportsRepository(ApplicationDbContext dbContext) : base(dbContext) { }

        public List<Reports> GetReportsByEvent(int eventId)
        {
            return _dbContext.Reports.Where(r => r.EventsId == eventId).ToList();
        }

        public Reports GetPredictionReport(int eventId)
        {
            return _dbContext.Reports.FirstOrDefault(r => r.EventsId == eventId && r.ReportType == "Prediction");
        }

        public Reports GetStatisticsReport(int eventId)
        {
            return _dbContext.Reports.FirstOrDefault(r => r.EventsId == eventId && r.ReportType == "Statistics");
        }
    }
}
