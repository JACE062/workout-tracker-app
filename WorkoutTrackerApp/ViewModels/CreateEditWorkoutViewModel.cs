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
        private Workout _currentWorkout;

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

        private async void LoadWorkoutAsync(int id)
        {
            
            CurrentWorkout = await _repository.GetWorkoutByIdAsync(id);
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
            if (CurrentWorkout.Title is null)
            {
                CurrentWorkout.Title = "Untitled Workout";
            } else if (CurrentWorkout.Description is null)
            {
                CurrentWorkout.Description = "Empty Description";
            }
            
            await _repository.UpdateWorkoutAsync(CurrentWorkout);
        }
    }
}
