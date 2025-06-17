using absence_tracker.Data;
using absence_tracker.Models;

namespace absence_tracker.Services
{
    public class UserService : IUserService
    {
        private readonly AbsenceTrackerDbContext _context;
        public UserService(AbsenceTrackerDbContext context)
        {
            _context = context;
        }

        public IEnumerable<User> GetAllUsers()
        {
            return _context.Users.ToList();
        }
    }
}
