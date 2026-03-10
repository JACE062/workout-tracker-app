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
    [QueryProperty(nameof(WorkoutId), "workoutId")]
    public class CreateEditWorkoutViewModel : BindableObject
    {
        private readonly IWorkoutRepository _repository;

        
        private int _workoutId;
        private Workout _originalWorkout;
        private Workout _currentWorkout;
        private Session _currentSession;

        public ICommand SaveReturnCommand { get; }
        public ICommand DiscardReturnCommand { get; }

        public CreateEditWorkoutViewModel(IWorkoutRepository repository)
        {
            _repository = repository;

            SaveReturnCommand = new Command(async () => await SaveAndReturn());
            DiscardReturnCommand = new Command(async () => await DiscardAndReturn());
        }

        public int WorkoutId
        {
            get => _workoutId;
            set
            {
                _workoutId = value;
                LoadWorkoutAsync(_workoutId);
            }
        }

        public Workout CurrentWorkout
        {
            get => _currentWorkout;
            set
            {
                _currentWorkout = value;
                OnPropertyChanged();
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

        public TimeSpan StartTimeSpan
        {
            get
            {
                if (CurrentWorkout == null) return DateTime.Now.TimeOfDay;
                return CurrentWorkout.StartTime.TimeOfDay;
            }
            set
            {
                if (CurrentWorkout == null || CurrentSession == null) return;
                var sessionDate = CurrentSession.Date;
                CurrentWorkout.StartTime = sessionDate.ToDateTime(TimeOnly.FromTimeSpan(value));

                AdjustForEndTimeMidnight();

                OnPropertyChanged();
                OnPropertyChanged(nameof(EndTimeSpan));
            }
        }

        public TimeSpan EndTimeSpan
        {
            get
            {
                if (CurrentWorkout == null) return DateTime.Now.AddHours(1).TimeOfDay;
                return CurrentWorkout.EndTime.TimeOfDay;
            }
            set
            {
                if (CurrentWorkout == null || CurrentSession == null) return;
                var sessionDate = CurrentSession.Date;
                CurrentWorkout.EndTime = sessionDate.ToDateTime(TimeOnly.FromTimeSpan(value));

                AdjustForEndTimeMidnight();

                OnPropertyChanged();
            }
        }

        public string SetsText
        {
            get
            {
                if (CurrentWorkout == null) return "0";
                return CurrentWorkout.Sets.ToString();
            }
            set
            {
                if (CurrentWorkout == null) return;

                if (string.IsNullOrWhiteSpace(value))
                {
                    CurrentWorkout.Sets = 0;
                }
                else if (int.TryParse(value, out int parsedSets))
                {
                    CurrentWorkout.Sets = parsedSets;
                }

                OnPropertyChanged();
            }
        }

        public string RepsText
        {
            get
            {
                if (CurrentWorkout == null) return "0";
                return CurrentWorkout.Reps.ToString();
            }
            set
            {
                if (CurrentWorkout == null) return;

                if (string.IsNullOrWhiteSpace(value))
                {
                    CurrentWorkout.Reps = 0;
                }
                else if (int.TryParse(value, out int parsedReps))
                {
                    CurrentWorkout.Reps = parsedReps;
                }

                OnPropertyChanged();
            }
        }

        private void AdjustForEndTimeMidnight()
        {
            var startTimeOfDay = CurrentWorkout.StartTime.TimeOfDay;
            var endTimeOfDay = CurrentWorkout.EndTime.TimeOfDay;

            if (endTimeOfDay < startTimeOfDay)
            {
                CurrentWorkout.EndTime = CurrentSession.Date.ToDateTime(TimeOnly.FromTimeSpan(endTimeOfDay)).AddDays(1);
            }
        }

        private async void LoadWorkoutAsync(int id)
        {
            _originalWorkout = await _repository.GetWorkoutByIdAsync(id);

            CurrentSession = await _repository.GetSessionByIdAsync(_originalWorkout.SessionId);

            CurrentWorkout = new Workout
            {
                WorkoutId = _originalWorkout.WorkoutId,
                SessionId = _originalWorkout.SessionId,
                Title = _originalWorkout.Title,
                Description = _originalWorkout.Description,
                StartTime = _originalWorkout.StartTime,
                EndTime = _originalWorkout.EndTime,
                Sets = _originalWorkout.Sets,
                Reps = _originalWorkout.Reps,
                Distance = _originalWorkout.Distance
            };

            OnPropertyChanged(nameof(CurrentWorkout));
            OnPropertyChanged(nameof(StartTimeSpan));
            OnPropertyChanged(nameof(EndTimeSpan));
            OnPropertyChanged(nameof(SetsText));
            OnPropertyChanged(nameof(RepsText));
        }

        public async Task SaveAndReturn()
        {
            await SaveCurrentWorkout();
            await Shell.Current.GoToAsync($"{nameof(SessionView)}?sessionId={CurrentWorkout.SessionId}");
        }

        public async Task DiscardAndReturn()
        {
            await Shell.Current.GoToAsync($"{nameof(SessionView)}?sessionId={CurrentWorkout.SessionId}");
        }

        private async Task SaveCurrentWorkout()
        {
            if (string.IsNullOrWhiteSpace(CurrentWorkout.Title))
                CurrentWorkout.Title = "Untitled Workout";

            if (string.IsNullOrWhiteSpace(CurrentWorkout.Description))
                CurrentWorkout.Description = "Empty Description";
            
            if (string.IsNullOrWhiteSpace(CurrentWorkout.Distance))
                CurrentWorkout.Distance = "";

            _originalWorkout.Title = CurrentWorkout.Title;
            _originalWorkout.Description = CurrentWorkout.Description;
            _originalWorkout.StartTime = CurrentWorkout.StartTime;
            _originalWorkout.EndTime = CurrentWorkout.EndTime;
            _originalWorkout.Sets = CurrentWorkout.Sets;
            _originalWorkout.Reps = CurrentWorkout.Reps;
            _originalWorkout.Distance = CurrentWorkout.Distance;

            await _repository.UpdateWorkoutAsync(_originalWorkout);
        }
    }
}
