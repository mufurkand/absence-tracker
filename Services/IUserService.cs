using absence_tracker.Models;

namespace absence_tracker.Services
{
    public interface IUserService
    {
        IEnumerable<User> GetAllUsers();
    }
}
