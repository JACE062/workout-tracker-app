using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WorkoutTracker.Data;
using WorkoutTracker.Data.Repositories;
using WorkoutTrackerApp.Views;

namespace WorkoutTrackerApp.ViewModels
{
    public class TestingUserViewModel : BindableObject
    {
        private readonly IWorkoutRepository _repository;

        public ICommand RegisterUserCommand { get; }
        public ICommand LoginRedirectCommand => new Command(async () => await LoginRedirect());

        public TestingUserViewModel(IWorkoutRepository repository) 
        {

            _repository = repository;

            RegisterUserCommand = new Command(async () => await GoToRegisterViewAsync());
        }

        private async Task GoToRegisterViewAsync()
        {
            await Shell.Current.GoToAsync(nameof(RegisterView));
        }

        public async Task LoginRedirect()
        {
            await Shell.Current.GoToAsync(nameof(LoginView));
        }

        public async Task LoadUser(string userId)
        {
            int id;
            try
            {
                id = int.Parse(userId);
            } catch (Exception ex) 
            {
                Console.WriteLine(ex.Message);
                return;
            }
            User? loadedUser = await _repository.GetUserByIdAsync(id);

            if (loadedUser == null)
            {
                Console.WriteLine($"User with id:{id} not found");
                return;
            }

            await Shell.Current.GoToAsync($"{nameof(SessionsListView)}?userId={loadedUser.UserId}");
        }
    }

}
