namespace KoiShowManagementSystem.Services.Services
{
    public interface IKoiService
    {
        List<Koi> GetAllKoi();
        Koi GetKoiById(int koiId);
        void CreateKoi(Koi koi);
        void UpdateKoi(Koi koi);
        void DeleteKoi(int koiId); // Xóa cá Koi
        List<Koi> GetKoiByUserId(int userId);
    }

    public class KoiService : IKoiService
    {
        private readonly IKoiRepository _koiRepository;

        // Khởi tạo KoiService với repository của cá Koi
        public KoiService(IKoiRepository koiRepository)
        {
            _koiRepository = koiRepository;
        }

        // Lấy tất cả các cá Koi
        public List<Koi> GetAllKoi()
        {
            return _koiRepository.GetAllKoi();
        }

        // Lấy cá Koi theo ID
        public Koi GetKoiById(int koiId)
        {
            return _koiRepository.GetKoiById(koiId);
        }

        // Lấy cá Koi của người dùng theo UserId
        public List<Koi> GetKoiByUserId(int userId)
        {
            return _koiRepository.GetKoiByUserId(userId);
        }

        // Tạo mới cá Koi
        public void CreateKoi(Koi koi)
        {
            _koiRepository.Add(koi); // Thêm cá Koi vào repository
        }

        // Cập nhật thông tin cá Koi
        public void UpdateKoi(Koi koi)
        {
            _koiRepository.Update(koi); // Cập nhật cá Koi trong repository
        }

        // Xóa cá Koi theo ID
        public void DeleteKoi(int koiId)
        {
            var koiToDelete = _koiRepository.GetKoiById(koiId); // Tìm cá Koi theo ID
            if (koiToDelete != null)
            {
                _koiRepository.Delete(koiToDelete); // Gọi phương thức Delete để xóa cá Koi
            }
        }
    }
}
