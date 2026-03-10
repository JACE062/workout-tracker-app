using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
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
        public string Distance {  get; set; }

        public bool IsRepBased => Sets > 0;
        public bool IsDistanceBased => Distance != "";

        public int SessionId { get; set; }

        [ForeignKey(nameof(SessionId))]
        public Session? Session { get; set; }



    }
}
