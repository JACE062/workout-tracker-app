using WorkoutTrackerApp.ViewModels;

namespace WorkoutTrackerApp.Views;

public partial class RegisterView : ContentPage
{
	public RegisterView(RegisterViewModel registerViewModel)
	{
		InitializeComponent();
		BindingContext = registerViewModel;
    }
}