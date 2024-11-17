namespace KoiShowManagementSystem.Repositories.Entity
{
    public class Reports
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int EventsId { get; set; }
        public Events Events { get; set; }
        [Column(TypeName = "nvarchar(100)")]
        public string ReportType { get; set; }
        [Column(TypeName = "nvarchar(100)")]
        public string ReportData { get; set; }
    }
}
