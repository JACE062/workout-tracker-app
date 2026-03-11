using Microsoft.EntityFrameworkCore;
using WorkoutTracker.Data;

namespace WorkoutTrackerApp
{
    public partial class App : Application
    {
        private readonly WorkoutDbContext _dbContext;

        public App(WorkoutDbContext dbContext)
        {
            InitializeComponent();
            _dbContext = dbContext;
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            var window = new Window(new AppShell());
            _ = InitializeDatabaseAsync();
            return window;
        }

        private async Task InitializeDatabaseAsync()
        {
            try
            {
                await _dbContext.Database.MigrateAsync();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Database migration failed: {ex}");
            }
        }
    }
}