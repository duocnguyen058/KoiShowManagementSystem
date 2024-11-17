using KoiShowManagementSystem.Repositories.Entity;

namespace KoiShowManagementSystem.ViewModels
{
    public class EventDetailsViewModel
    {
        public Events EventDetail { get; set; }
        public bool CanRegister { get; set; }
        public bool IsRegistered { get; set; }
        public List<RankedKoiViewModel> RankedKoiList { get; set; }
    }
}
