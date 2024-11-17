namespace KoiShowManagementSystem.ViewModels
{
    public class ScoreEventViewModel
    {
        public int ParticipationId { get; set; }
        public string KoiName { get; set; }
        public string Category { get; set; }
        public float CurrentScore { get; set; }
        public string? PhotoPath { get; set; }
    }

}
