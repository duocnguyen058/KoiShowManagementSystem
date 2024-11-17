namespace KoiShowManagementSystem.Repositories.Entity
{
    public class Scores
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int Event_Koi_ParticipationId { get; set; }
        public Event_Koi_Participation Event_Koi_Participations { get; set; }
        public int UsersId { get; set; }
        public Users Users { get; set; }
        public float ShapeScore { get; set; } //50%
        public float ColorScore { get; set; } //30%
        public float PatternScore { get; set; } //20%
        public float TotalScore { get; set; }
    }
}
