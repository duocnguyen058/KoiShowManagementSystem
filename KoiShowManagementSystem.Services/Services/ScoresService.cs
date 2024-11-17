namespace KoiShowManagementSystem.Services.Services
{
    public interface IScoresService
    {
        void AddScore(int eventKoiId, int judgeId, float shapeScore, float colorScore, float patternScore);
        List<Scores> GetScoresByEvent(int eventId);
        bool CheckScoreExists(int eventKoiId, int judgeId);
    }

    public class ScoresService : IScoresService
    {
        private readonly IScoresRepository _scoresRepository;
        private readonly IEventKoiParticipationRepository _eventKoiParticipationRepository;
        // Constructor nhận IScoresRepository
        public ScoresService(IScoresRepository scoresRepository, IEventKoiParticipationRepository eventKoiParticipationRepository)
        {
            _scoresRepository = scoresRepository ?? throw new ArgumentNullException(nameof(scoresRepository));
            _eventKoiParticipationRepository = eventKoiParticipationRepository;
        }

        public void AddScore(int eventKoiId, int judgeId, float shapeScore, float colorScore, float patternScore)
        {
            // Tính điểm tổng
            var totalScore = shapeScore * 0.5f + colorScore * 0.3f + patternScore * 0.2f;

            // Kiểm tra xem điểm đã tồn tại cho giám khảo và cá Koi này chưa
            if (CheckScoreExists(eventKoiId, judgeId))
            {
                throw new Exception("Giám khảo đã chấm điểm cho cá Koi này rồi.");
            }

            // Tạo đối tượng Scores mới
            var score = new Scores
            {
                Event_Koi_ParticipationId = eventKoiId,
                UsersId = judgeId,
                ShapeScore = shapeScore,
                ColorScore = colorScore,
                PatternScore = patternScore,
                TotalScore = totalScore
            };

            // Thêm vào cơ sở dữ liệu
            _scoresRepository.Add(score);
            var eventKoiParticipation = _eventKoiParticipationRepository.GetById(eventKoiId);
            if (eventKoiParticipation != null)
            {
                eventKoiParticipation.Score = totalScore;
                _eventKoiParticipationRepository.Update(eventKoiParticipation);
            }
            else
            {
                throw new Exception("Không tìm thấy tham gia sự kiện cho cá Koi này.");
            }

        }
        // Kiểm tra xem điểm đã tồn tại chưa
        public bool CheckScoreExists(int eventKoiId, int judgeId)
        {
            var score = _scoresRepository.GetScoreByEventKoiAndJudge(eventKoiId, judgeId);
            return score != null;
        }
        public List<Scores> GetScoresByEvent(int eventId)
        {
            return _scoresRepository.GetScoresByEvent(eventId);
        }
    }
}
