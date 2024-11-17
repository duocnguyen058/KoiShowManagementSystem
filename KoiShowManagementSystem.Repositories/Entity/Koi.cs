namespace KoiShowManagementSystem.Repositories.Entity
{
    public class Koi
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int UsersId { get; set; }
        public string Name { get; set; }
        [Column(TypeName = "nvarchar(100)")]
        public string Variety { get; set; }
        public float Size { get; set; }
        public int Age { get; set; }
        [Column(TypeName = "nvarchar(100)")]
        public string RegistrationStatus { get; set; }
        public Users? Users { get; set; }
        public string? PhotoPath { get; set; }
    }
}
