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
    [QueryProperty(nameof(SessionId), "sessionId")]
    public class SessionViewModel : BindableObject
    {
        private readonly IWorkoutRepository _repository;

        private int _sessionId; 
        private Session _currentSession;
        private String _title;
        public ObservableCollection<Workout> DisplayedWorkouts { get; set; } = new();

        public ICommand WorkoutTappedCommand { get; }
        public ICommand CreateWorkoutCommand { get; }
        public ICommand DeleteWorkoutCommand { get; }
        public ICommand ReturnCommand { get; }
 
        public SessionViewModel(IWorkoutRepository repository)
        {
            _repository = repository;

            WorkoutTappedCommand = new Command<Workout>(async (selectedWorkout) => await OnWorkoutTapped(selectedWorkout));
            CreateWorkoutCommand = new Command(async () => await CreateNewWorkout());
            DeleteWorkoutCommand = new Command<Workout>(async (workoutToDelete) => await DeleteWorkout(workoutToDelete));
            ReturnCommand = new Command(async () => await ReturnToSessionList());
        }

        public int SessionId
        {
            get => _sessionId;
            set
            {
                _sessionId = value;
                LoadSessionAsync(_sessionId);
            }
        }

        public Session CurrentSession
        {
            get => _currentSession;
            set
            {
                _currentSession = value;
                OnPropertyChanged(); 
            }
        }

        public String Title
        {
            get => _title;
            set
            {
                if (_title != value)
                {
                    _title = value;
                    OnPropertyChanged();
                }
            }
        }

        public DateTime SessionDateUI
        {
            get  
            {
                if (CurrentSession == null) return DateTime.Now;

                return CurrentSession.Date.ToDateTime(TimeOnly.MinValue); 
            }
            set
            {
                CurrentSession.Date = DateOnly.FromDateTime(value);
                OnPropertyChanged();
            }
        }

        private async void LoadSessionAsync(int id)
        {
            CurrentSession = await _repository.GetSessionByIdAsync(id);
            Title = CurrentSession.Title;
            

            DisplayedWorkouts.Clear();
            if (CurrentSession?.Workouts != null)
            {
                foreach (Workout workout in CurrentSession.Workouts)
                {
                    DisplayedWorkouts.Add(workout);
                }
            }
            OnPropertyChanged(nameof(SessionDateUI));
        }

        private async Task OnWorkoutTapped(Workout selectedWorkout)
        {
            if (selectedWorkout == null) return;

            await SaveCurrentSession();
            await Shell.Current.GoToAsync($"{nameof(CreateEditWorkoutView)}?workoutId={selectedWorkout.WorkoutId}");
        }

        public async Task CreateNewWorkout()
        {
            Workout newWorkout = new Workout();
            newWorkout.Title = "New Workout";
            newWorkout.Description = "Empty Workout, let's get something done!";
            newWorkout.StartTime = DateTime.Now;
            newWorkout.EndTime = DateTime.Now.AddHours(1);
            newWorkout.Distance = "";
            newWorkout.SessionId = SessionId;


            await _repository.AddWorkoutAsync(newWorkout);

            await SaveCurrentSession();
            await Shell.Current.GoToAsync($"{nameof(CreateEditWorkoutView)}?workoutId={newWorkout.WorkoutId}");
        }

        private async Task DeleteWorkout(Workout workoutToDelete)
        {
            if (workoutToDelete == null) return;

            bool isCofirmed = await Shell.Current.DisplayAlert(
                "Delete Workout",
                $"Are you sure you want to delete '{workoutToDelete.Title}'?",
                "Yes, Delete",
                "Cancel");

            if (!isCofirmed) return;

            DisplayedWorkouts.Remove(workoutToDelete);
            await _repository.DeleteWorkoutAsync(workoutToDelete);
        }

        public void ChangeTitle(string entryText)
        {
            CurrentSession.Title = entryText;
        }

        public async Task ReturnToSessionList()
        {
            await SaveCurrentSession();
            await Shell.Current.GoToAsync($"{nameof(SessionsListView)}?userId={CurrentSession.UserId}");
        }

        private async Task SaveCurrentSession()
        {
            if (string.IsNullOrWhiteSpace(Title))
                CurrentSession.Title = "(Empty Title)";
            else
                CurrentSession.Title = Title;

            CurrentSession.Date = DateOnly.FromDateTime(SessionDateUI);
                
            await _repository.UpdateSessionAsync(CurrentSession);
        }

    }
}
