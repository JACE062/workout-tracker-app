using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkoutTracker.Data
{
    public class Workout
    {
        public int WorkoutId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }

        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public int Sets { get; set; }
        public int Reps { get; set; }
        public int Distance {  get; set; }

        public int SessionId { get; set; }
        public Session Session { get; set; }



    }
}
