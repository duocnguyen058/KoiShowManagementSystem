namespace KoiShowManagementSystem.Repositories.Entity
{
    //(Tham gia sự kiện của Koi)
    public class Event_Koi_Participation
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int EventsId { get; set; }
        public int KoiId { get; set; }

        [Column(TypeName = "nvarchar(100)")]
        public string Category { get; set; }//Hạng mục
        public float Score { get; set; }
        public Events Events { get; set; }
        public Koi Kois { get; set; }
    }
}
