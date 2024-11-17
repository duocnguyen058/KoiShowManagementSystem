using KoiShowManagementSystem.Repositories.Entity;

namespace KoiShowManagementSystem.ViewModels
{
    public class RefereeViewModel
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public int AssignedEventCount { get; set; }
    }

    public class AssignJudgeViewModel
    {
        public int UserId { get; set; }
        public List<Events> Events { get; set; } // Danh sách sự kiện sắp diễn ra hoặc đang diễn ra
        public List<int> SelectedEventIds { get; set; } // Sự kiện được chọn để phân công
        public List<int> AssignedEventIds { get; set; }
    }
}
