using WorkoutTracker.Data;

namespace WorkoutTrackerApp
{
    public partial class App : Application
    {
        public App(WorkoutDbContext dbContext)
        {
            InitializeComponent();
            dbContext.Database.EnsureCreated();
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            return new Window(new AppShell());
        }
    }
}