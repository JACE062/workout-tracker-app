using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkoutTracker.Data.Repositories
{
    public interface IWorkoutRepository
    {
        Task<List<Workout>> GetAllWorkoutsForUserAsync(int userId);
        Task<Workout?> GetWorkoutByIdAsync(int id);
        Task AddWorkoutAsync(Workout workout);
        Task UpdateWorkoutAsync(Workout workout);
        Task DeleteWorkoutAsync(Workout workout);

        Task<List<Session>> GetAllSessionsForUserAsync(int userId);
        Task<Session?> GetSessionByIdAsync(int id);
        Task AddSessionAsync(Session session);
        Task UpdateSessionAsync(Session session);
        Task DeleteSessionAsync(Session session);

        Task<List<User>> GetAllUsersAsync();
        Task<User?> GetUserByIdAsync(int id);
        Task AddUserAsync(User user);
        Task UpdateUserAsync(User user);
        Task DeleteUserAsync(User user);

    }
}
