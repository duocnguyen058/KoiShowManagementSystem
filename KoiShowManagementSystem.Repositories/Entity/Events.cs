namespace KoiShowManagementSystem.Repositories.Entity
{
    public class Events
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập tên sự kiện")]
        [Column(TypeName = "nvarchar(max)")]
        public string EventName { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập ngày bắt đầu")]
        public DateTime StartDate { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập ngày kết thúc")]
        public DateTime EndDate { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập địa điểm")]
        [Column(TypeName = "nvarchar(max)")]
        public string Location { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập miêu tả")]
        [Column(TypeName = "nvarchar(max)")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn trạng thái")]
        [Column(TypeName = "nvarchar(100)")]
        public string Status { get; set; } = "Upcoming";
    }
}
