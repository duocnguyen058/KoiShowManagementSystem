namespace KoiShowManagementSystem.Repositories.Entity
{
    //(Phân công giám khảo)
    public class JudgeAssignments
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int UsersId { get; set; }
        public Users? Users { get; set; }
        public int EventsId { get; set; }
        public Events? Events { get; set; }
    }
}
