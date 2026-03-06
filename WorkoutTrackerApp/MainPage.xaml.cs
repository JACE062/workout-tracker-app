using WorkoutTracker.Data;
using WorkoutTracker.Data.Repositories;
using WorkoutTrackerApp.ViewModels;

namespace WorkoutTrackerApp
{
    public partial class MainPage : ContentPage
    {
        int count = 0;

        private readonly WorkoutListViewModel _viewModel;

        public MainPage(WorkoutListViewModel viewModel)
        {
            InitializeComponent();
            _viewModel = viewModel;

            BindingContext = _viewModel;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            await _viewModel.LoadWorkoutsAsync();
        }


    }
}
