using System.Windows.Input;
using WorkoutTrackerApp.Views;
using WorkoutTracker.Data.Repositories;
using WorkoutTracker.Data;

namespace WorkoutTrackerApp.ViewModels
{
    public class RegisterViewModel : BindableObject
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

        public ICommand RegisterCommand { get; }

        public ICommand ToLoginCommand => new Command(async () => await GoToLoginViewAsync());

        public RegisterViewModel(IWorkoutRepository repository)
        {
            _repository = repository;
            RegisterCommand = new Command(async () => await RegisterUser());
        }

        private async Task RegisterUser()
        {
            if (string.IsNullOrWhiteSpace(Name) ||
                string.IsNullOrWhiteSpace(Username) ||
                string.IsNullOrWhiteSpace(Password))
            {
                ResultMessage = "All fields are required.";
                return;
            }

            User usernameTaken = await _repository.GetUserByUsernameAsync(Username.Trim());
            if (usernameTaken != null)
            {
                ResultMessage = "Username is already taken. Please choose another.";
                return;
            }

            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(Password);
            var user = new User(Name, Username, hashedPassword);
            await _repository.AddUserAsync(user);
            ResultMessage = "User created successfully!";
            await GoToLoginViewAsync();
        }

        private async Task GoToLoginViewAsync()
        {
            await Shell.Current.GoToAsync(nameof(LoginView));
        }
    }
}
