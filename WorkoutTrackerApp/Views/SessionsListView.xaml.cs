using WorkoutTracker.Data;
using WorkoutTracker.Data.Repositories;
using WorkoutTrackerApp.ViewModels;

namespace WorkoutTrackerApp.Views;

public partial class SessionsListView : ContentPage
{
	public readonly SessionsListViewModel _viewModel;

	public SessionsListView(SessionsListViewModel viewModel)
	{
        InitializeComponent();

        _viewModel = viewModel;

        BindingContext = _viewModel;
    }

}