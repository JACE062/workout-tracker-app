using WorkoutTracker.Data;
using WorkoutTracker.Data.Repositories;
using WorkoutTrackerApp.ViewModels;

namespace WorkoutTrackerApp.Views;

public partial class SessionView : ContentPage
{

    private readonly SessionViewModel _viewModel;

    public SessionView(SessionViewModel viewModel)
	{
		InitializeComponent();

		_viewModel = viewModel;

		BindingContext = _viewModel;
	}
}