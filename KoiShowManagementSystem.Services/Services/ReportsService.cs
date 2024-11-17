namespace KoiShowManagementSystem.Services.Services
{
    public interface IReportsService
    {
        Reports GeneratePredictionReport(int eventId);
        Reports GenerateStatisticsReport(int eventId);
        List<Reports> GetReportsByEvent(int eventId);
    }

    public class ReportsService : IReportsService
    {
        private readonly IReportsRepository _reportsRepository;
        private readonly IScoresRepository _scoresRepository;
        private readonly IEventKoiParticipationRepository _eventKoiParticipationRepository;

        public ReportsService(
            IReportsRepository reportsRepository,
            IScoresRepository scoresRepository,
            IEventKoiParticipationRepository eventKoiParticipationRepository)
        {
            _reportsRepository = reportsRepository;
            _scoresRepository = scoresRepository;
            _eventKoiParticipationRepository = eventKoiParticipationRepository;
        }

        public Reports GeneratePredictionReport(int eventId)
        {
            // Giả định rằng dự đoán dựa trên điểm trung bình của các tiêu chí chấm điểm
            var scores = _scoresRepository.GetScoresByEvent(eventId);
            var predictionData = scores
                .GroupBy(s => s.Event_Koi_ParticipationId)
                .Select(g => new
                {
                    KoiId = g.Key,
                    AverageScore = g.Average(s => s.TotalScore)
                })
                .OrderByDescending(x => x.AverageScore)
                .ToList();

            var reportData = Newtonsoft.Json.JsonConvert.SerializeObject(predictionData);

            var report = new Reports
            {
                EventsId = eventId,
                ReportType = "Prediction",
                ReportData = reportData
            };

            _reportsRepository.Add(report);
            return report;
        }

        public Reports GenerateStatisticsReport(int eventId)
        {
            // Thống kê dựa trên tổng số lượng cá Koi tham gia và điểm số trung bình của từng tiêu chí
            var participants = _eventKoiParticipationRepository.GetParticipantsByEvent(eventId);
            var scores = _scoresRepository.GetScoresByEvent(eventId);

            var statisticsData = new
            {
                TotalParticipants = participants.Count,
                AverageShapeScore = scores.Average(s => s.ShapeScore),
                AverageColorScore = scores.Average(s => s.ColorScore),
                AveragePatternScore = scores.Average(s => s.PatternScore),
                AverageTotalScore = scores.Average(s => s.TotalScore)
            };

            var reportData = Newtonsoft.Json.JsonConvert.SerializeObject(statisticsData);

            var report = new Reports
            {
                EventsId = eventId,
                ReportType = "Statistics",
                ReportData = reportData
            };

            _reportsRepository.Add(report);
            return report;
        }

        public List<Reports> GetReportsByEvent(int eventId)
        {
            return _reportsRepository.GetReportsByEvent(eventId);
        }
    }
}
