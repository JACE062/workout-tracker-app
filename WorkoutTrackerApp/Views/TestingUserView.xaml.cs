using WorkoutTracker.Data;
using WorkoutTracker.Data.Repositories;
using WorkoutTrackerApp.ViewModels;

namespace WorkoutTrackerApp.Views;

public partial class TestingUserView : ContentPage
{
    public readonly TestingUserViewModel _viewModel;

    public TestingUserView(TestingUserViewModel viewModel)
	{
		InitializeComponent();

        _viewModel = viewModel;

        BindingContext = _viewModel;
    }
}