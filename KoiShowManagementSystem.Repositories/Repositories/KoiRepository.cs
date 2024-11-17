namespace KoiShowManagementSystem.Repositories.Repositories
{
    public interface IKoiRepository : IRepository<Koi>
    {
        List<Koi> GetAllKoi();
        Koi GetKoiById(int koiId);
        List<Koi> GetKoiByUserId(int userId);
        void Delete(Koi koi); // Thêm phương thức Delete vào interface
    }

    public class KoiRepository : RepositoryBase<Koi>, IKoiRepository
    {
        public KoiRepository(ApplicationDbContext dbContext) : base(dbContext) { }

        public List<Koi> GetAllKoi()
        {
            return _dbContext.Kois.ToList();
        }

        public Koi GetKoiById(int koiId)
        {
            return _dbContext.Kois.Find(koiId);
        }

        public List<Koi> GetKoiByUserId(int userId)
        {
            return _dbContext.Kois.Where(k => k.UsersId == userId).ToList();
        }

        // Phương thức Delete đã được thêm vào trong lớp KoiRepository
        public void Delete(Koi koi)
        {
            _dbContext.Kois.Remove(koi); // Xóa cá Koi khỏi DbContext
            _dbContext.SaveChanges(); // Lưu thay đổi vào cơ sở dữ liệu
        }
    }
}
