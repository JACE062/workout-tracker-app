using WorkoutTracker.Data;
using WorkoutTracker.Data.Repositories;
using WorkoutTrackerApp.ViewModels;
using WorkoutTrackerApp.Views;

namespace WorkoutTrackerApp
{
    public partial class MainPage : ContentPage
    {
        int count = 0;

        public MainPage()
        {
            InitializeComponent();
        }

        private async void OnUserButtonClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync($"{nameof(UserView)}");
        }

    }
}
