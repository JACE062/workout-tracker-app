using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WorkoutTracker.Data;
using WorkoutTracker.Data.Repositories;
using WorkoutTrackerApp.Views;

namespace WorkoutTrackerApp.ViewModels
{
    [QueryProperty(nameof(UserId), "userId")]
    public class SessionsListViewModel : BindableObject
    {
        private readonly IWorkoutRepository _repository;

        private int _userId;

        public ObservableCollection<Session> UserSessions { get; set; } = new();

        public ICommand SessionTappedCommand { get; }
        public ICommand DeleteSessionCommand { get; }
        public ICommand CreateSessionCommand { get; }
        public ICommand LogOutCommand { get; }

        public SessionsListViewModel(IWorkoutRepository repository)
        {
            _repository = repository;

            SessionTappedCommand = new Command<Session>(async (selectedSession) => await OnSessionTapped(selectedSession));
            DeleteSessionCommand = new Command<Session>(async (sessionToDelete) => await DeleteSession(sessionToDelete));
            CreateSessionCommand = new Command(async () => await CreateNewSession());
            LogOutCommand = new Command(async () => await LogOut());
        }

        public int UserId
        {
            get => _userId;
            set
            {
                _userId = value;
                _ = LoadSessionsAsync(_userId);
            }
        }

        public async Task LoadSessionsAsync(int userId)
        {
            try
            {
                var dbUserSessions = await _repository.GetAllSessionsForUserAsync(userId);

                UserSessions.Clear();
                foreach (var session in dbUserSessions)
                {
                    UserSessions.Add(session);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Failed to load sessions for user {userId}: {ex}");
                UserSessions.Clear();
            }
        }

        private async Task OnSessionTapped(Session selectedSession)
        {
            if (selectedSession == null) return;

            await Shell.Current.GoToAsync($"{nameof(SessionView)}?sessionId={selectedSession.SessionId}");
        }

        

        public async Task CreateNewSession()
        {
            Session newSession = new Session();
            newSession.Title = "New Session";
            newSession.Date = DateOnly.FromDateTime(DateTime.Now);
            newSession.UserId = _userId;

            await _repository.AddSessionAsync(newSession);

            UserSessions.Add(newSession);

            await Shell.Current.GoToAsync($"{nameof(SessionView)}?sessionId={newSession.SessionId}");
        }

        private async Task DeleteSession(Session sessionToDelete)
        {
            if (sessionToDelete == null) return;

            bool isCofirmed = await Shell.Current.DisplayAlert(
                "Delete Session",
                $"Are you sure you want to delete '{sessionToDelete.Title}'? All associated workouts will be permanently lost.",
                "Yes, Delete",
                "Cancel");

            if (!isCofirmed) return;

            UserSessions.Remove(sessionToDelete);
            await _repository.DeleteSessionAsync(sessionToDelete);
        }

        private async Task LogOut()
        {
            await Shell.Current.GoToAsync($"{nameof(LoginView)}");
        }
    }
}
