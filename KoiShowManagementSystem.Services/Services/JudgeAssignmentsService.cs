using KoiShowManagementSystem.Repositories;
using KoiShowManagementSystem.Repositories.Entity;
using KoiShowManagementSystem.Repositories.Repositories;
using Microsoft.EntityFrameworkCore;

namespace KoiShowManagementSystem.Services.Services
{
    public interface IJudgeAssignmentsService
    {
        string  AssignJudgeToEvent(int eventId, int judgeId);
        bool IsJudgeAssignedToEvent(int eventId, int judgeId); // Kiểm tra giám khảo đã được phân công chưa
        List<Events> GetEventsByJudge(int judgeId); // Lấy danh sách sự kiện mà giám khảo đã tham gia
        List<JudgeAssignments> GetEventsByJudgeId(int judgeId);
    }

    public class JudgeAssignmentsService : IJudgeAssignmentsService
    {
        private readonly IJudgeAssignmentsRepository _judgeAssignmentsRepository;
        private readonly ApplicationDbContext _context;

        public JudgeAssignmentsService(IJudgeAssignmentsRepository judgeAssignmentsRepository, ApplicationDbContext context)
        {
            _judgeAssignmentsRepository = judgeAssignmentsRepository;
            _context = context;
        }

        public string AssignJudgeToEvent(int eventId, int judgeId)
        {
            if (IsJudgeAssignedToEvent(eventId, judgeId))
            {
                return $"Giám khảo đã được phân công cho sự kiện {eventId}.";
            }
            var assignment = new JudgeAssignments
            {
                EventsId = eventId,
                UsersId = judgeId
            };
            _judgeAssignmentsRepository.Add(assignment);
            return "Phân công giám khảo thành công.";
        }
        public bool IsJudgeAssignedToEvent(int eventId, int judgeId)
        {
            return _judgeAssignmentsRepository.Any(j => j.EventsId == eventId && j.UsersId == judgeId);
        }
        public List<Events> GetEventsByJudge(int judgeId)
        {
            var assignments = _judgeAssignmentsRepository.GetMany(j => j.UsersId == judgeId);
            return assignments.Select(a => a.Events).ToList();
        }
        public List<JudgeAssignments> GetEventsByJudgeId(int judgeId)
        {
            return _context.JudgeAssignments
                           .Where(ja => ja.UsersId == judgeId)
                           .Include(ja => ja.Events)
                           .ToList();
        }
    }
}
