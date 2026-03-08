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
        public ICommand CreateSessionCommand { get; }

        public SessionsListViewModel(IWorkoutRepository repository)
        {
            _repository = repository;

            SessionTappedCommand = new Command<Session>(async (selectedSession) => await OnSessionTapped(selectedSession));
            CreateSessionCommand = new Command(async () => await CreateNewSession());
        }

        public int UserId
        {
            get => _userId;
            set
            {
                _userId = value;
                LoadSessionsAsync(_userId);
            }
        }

        public async void LoadSessionsAsync(int userId)
        {
            var dbUserSessions = await _repository.GetAllSessionsForUserAsync(userId);

            UserSessions.Clear();
            foreach (var session in dbUserSessions)
            {
                UserSessions.Add(session);
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
            newSession.Timestamp = DateTime.Now;
            newSession.UserId = _userId;

            await _repository.AddSessionAsync(newSession);

            UserSessions.Add(newSession);

            await Shell.Current.GoToAsync($"{nameof(SessionView)}?sessionId={newSession.SessionId}");
        }
    }
}
