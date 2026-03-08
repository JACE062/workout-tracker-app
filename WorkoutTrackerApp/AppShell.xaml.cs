using WorkoutTrackerApp.Views;

namespace WorkoutTrackerApp
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            Routing.RegisterRoute(nameof(SessionsListView), typeof(SessionsListView));
            Routing.RegisterRoute(nameof(SessionView), typeof(SessionView));
            Routing.RegisterRoute(nameof(CreateEditWorkoutView), typeof(CreateEditWorkoutView));
        }
    }
}
