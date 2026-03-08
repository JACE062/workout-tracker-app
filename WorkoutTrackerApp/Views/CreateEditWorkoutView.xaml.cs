using WorkoutTracker.Data;
using WorkoutTracker.Data.Repositories;
using WorkoutTrackerApp.ViewModels;

namespace WorkoutTrackerApp.Views;

public partial class CreateEditWorkoutView : ContentPage
{

    private readonly CreateEditWorkoutViewModel _viewModel;

    public CreateEditWorkoutView(CreateEditWorkoutViewModel viewModel)
	{
		InitializeComponent();

        _viewModel = viewModel;

        BindingContext = _viewModel;
    }
}