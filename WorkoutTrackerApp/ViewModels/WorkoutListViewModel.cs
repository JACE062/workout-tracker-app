using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkoutTracker.Data;
using WorkoutTracker.Data.Repositories;

namespace WorkoutTrackerApp.ViewModels
{
    public  class WorkoutListViewModel
    {
        private readonly IWorkoutRepository _repository;

        public ObservableCollection<Workout> Workouts { get; set; } = new();

        public WorkoutListViewModel(IWorkoutRepository repository)
        {
            _repository = repository;
        }

        public async Task LoadWorkoutsAsync()
        {
            var dbWorkouts = await _repository.GetAllWorkoutsAsync();

            Workouts.Clear();
            foreach (var workout in dbWorkouts)
            {
                Workouts.Add(workout);
            }
        }


    }

}
