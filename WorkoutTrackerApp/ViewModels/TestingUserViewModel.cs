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

        public ICommand CreateUserCommand { get; }

        public TestingUserViewModel(IWorkoutRepository repository) 
        {

            _repository = repository;

            CreateUserCommand = new Command(async () => await CreateNewUser());
        }
        public async Task CreateNewUser()
        {
            //User newUser = new User();
            //newUser.Name = "Jonathan";
            //newUser.Username = "jace062";
            //newUser.Password = "$uperStrongP@55w0rd!";

            //await _repository.AddUserAsync(newUser);

            //await Shell.Current.GoToAsync($"{nameof(SessionsListView)}?userId={newUser.UserId}");
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
