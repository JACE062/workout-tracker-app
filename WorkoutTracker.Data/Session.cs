using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkoutTracker.Data
{
    public class Session
    {
        public int SessionId { get; set; }
        public string Title { get; set; }
        public DateTime Timestamp { get; set; }
        
        public int UserId { get; set; }

        [ForeignKey(nameof(UserId))]
        public User? User { get; set; }

        public List<Workout> Workouts { get; set; } = new();

    }
}
