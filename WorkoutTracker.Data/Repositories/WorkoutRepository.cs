using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Collections.Specialized.BitVector32;

namespace WorkoutTracker.Data.Repositories
{
    public class WorkoutRepository : IWorkoutRepository
    {
        private readonly WorkoutDbContext _dbContext;

        public WorkoutRepository(WorkoutDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        // Add logic
        public async Task AddSessionAsync(Session session)
        {
            await _dbContext.Sessions.AddAsync(session);
            await _dbContext.SaveChangesAsync();
        }

        public async Task AddUserAsync(User user)
        {
            await _dbContext.Users.AddAsync(user);
            await _dbContext.SaveChangesAsync();
        }

        public async Task AddWorkoutAsync(Workout workout)
        {
            await _dbContext.Workouts.AddAsync(workout);
            await _dbContext.SaveChangesAsync();
        }


        // Getall methods
        public async Task<List<Session>> GetAllSessionsForUserAsync(int userId)
        {
            return await _dbContext.Sessions
                .Where(s => s.UserId == userId)
                .ToListAsync();
        }

        public async Task<List<User>> GetAllUsersAsync()
        {
            return await _dbContext.Users.ToListAsync();
        }

        public async Task<List<Workout>> GetAllWorkoutsForUserAsync(int userId)
        {
            return await _dbContext.Workouts
                .Where(w => w.Session.UserId == userId)
                .ToListAsync();
        }


        // GetById methods
        public async Task<Session?> GetSessionByIdAsync(int id)
        {
            return await _dbContext.Sessions.Include(s => s.Workouts).FirstOrDefaultAsync(s => s.SessionId == id);
        }

        public async Task<User?> GetUserByIdAsync(int id)
        {
            return await _dbContext.Users.FindAsync(id);
        }

        public async Task<Workout?> GetWorkoutByIdAsync(int id)
        {
            return await _dbContext.Workouts.FindAsync(id);
        }


        // Update methods
        public async Task UpdateSessionAsync(Session session)
        {
            _dbContext.Sessions.Update(session);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateUserAsync(User user)
        {
            _dbContext.Users.Update(user);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateWorkoutAsync(Workout workout)
        {
            _dbContext.Workouts.Update(workout);
            await _dbContext.SaveChangesAsync();
        }


        // Delete methods
        public async Task DeleteSessionAsync(Session session)
        {
            _dbContext.Sessions.Remove(session);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteUserAsync(User user)
        {
            _dbContext.Users.Remove(user);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteWorkoutAsync(Workout workout)
        {
            _dbContext.Workouts.Remove(workout);
            await _dbContext.SaveChangesAsync();
        }
    }
}
