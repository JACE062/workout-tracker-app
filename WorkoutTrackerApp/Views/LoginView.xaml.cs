using WorkoutTrackerApp.ViewModels;

namespace WorkoutTrackerApp.Views;

public partial class LoginView : ContentPage
{
	public LoginView(LoginViewModel loginViewModel)
	{
		InitializeComponent();
		BindingContext = loginViewModel;
	}
}