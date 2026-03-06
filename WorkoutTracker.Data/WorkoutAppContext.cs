using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkoutTracker.Data
{
    public class WorkoutDbContext : DbContext
    {
        public DbSet<Workout> Workouts { get; set; }
        public DbSet<Session> Sessions { get; set; }
        public DbSet<User> Users { get; set; }

        public WorkoutDbContext(DbContextOptions<WorkoutDbContext> options) : base(options)
        {
            
        }
    }
}
