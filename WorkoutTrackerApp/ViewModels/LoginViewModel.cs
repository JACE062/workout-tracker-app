using System.Windows.Input;
using System.Linq;
using WorkoutTracker.Data.Repositories;
using WorkoutTracker.Data;
using WorkoutTrackerApp.Views;

namespace WorkoutTrackerApp.ViewModels
{
    public class LoginViewModel : BindableObject
    {
        private readonly IWorkoutRepository _repository;

        private string _name;
        private string _username;
        private string _password;
        private string _resultMessage;

        public string Name
        {
            get => _name;
            set { _name = value; OnPropertyChanged(); }
        }

        public string Username
        {
            get => _username;
            set { _username = value; OnPropertyChanged(); }
        }
        public string Password
        {
            get => _password;
            set { _password = value; OnPropertyChanged(); }
        }

        public string ResultMessage
        {
            get => _resultMessage;
            set { _resultMessage = value; OnPropertyChanged(); }
        }

        public ICommand LoginCommand { get; }
        public ICommand ToRegisterCommand => new Command(async () => await GoToRegisterViewAsync());


        public LoginViewModel(IWorkoutRepository repository)
        {
            _repository = repository;
            LoginCommand = new Command(async () => await LoginUser());
        }

        private async Task LoginUser()
        {
            if (string.IsNullOrWhiteSpace(Username) || string.IsNullOrWhiteSpace(Password))
            {
                ResultMessage = "All fields are required.";
                return;
            }

            var user = await _repository.GetUserByUsernameAsync(Username.Trim());
            if (user == null)
            {
                ResultMessage = "Invalid username or password.";
                return;
            }

            bool isValidPassword = await ValidateAndUpgradePasswordIfNeeded(user, Password);
            if (!isValidPassword)
            {
                ResultMessage = "Invalid username or password.";
                return;
            }

            if (Shell.Current == null)
            {
                ResultMessage = "Navigation is not available right now. Please try again.";
                return;
            }

            try
            {
                ResultMessage = "";
                await Shell.Current.GoToAsync($"{nameof(SessionsListView)}?userId={user.UserId}");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Sessions navigation failed for user {user.UserId}: {ex}");
                ResultMessage = "Login succeeded, but opening sessions failed. Please try again.";
            }
        }

        private async Task<bool> ValidateAndUpgradePasswordIfNeeded(User user, string enteredPassword)
        {
            if (string.IsNullOrWhiteSpace(user.Password))
            {
                return false;
            }

            bool looksLikeBcryptHash = user.Password.StartsWith("$2a$") || user.Password.StartsWith("$2b$") || user.Password.StartsWith("$2y$");

            if (looksLikeBcryptHash)
            {
                return BCrypt.Net.BCrypt.Verify(enteredPassword, user.Password);
            }

            if (user.Password == enteredPassword)
            {
                user.Password = BCrypt.Net.BCrypt.HashPassword(enteredPassword);
                await _repository.UpdateUserAsync(user);
                return true;
            }

            return false;
        }

        private async Task GoToRegisterViewAsync()
        {
            await Shell.Current.GoToAsync(nameof(RegisterView));
        }
    }
}
