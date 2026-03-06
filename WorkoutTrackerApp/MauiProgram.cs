using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore; 
using WorkoutTracker.Data;
using WorkoutTracker.Data.Repositories;
using WorkoutTrackerApp.ViewModels;

namespace WorkoutTrackerApp
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });
            string dbPath = Path.Combine(FileSystem.AppDataDirectory, "workout.db");

            
            builder.Services.AddDbContext<WorkoutDbContext>(options =>
                options.UseSqlite($"Filename={dbPath}"));
            builder.Services.AddScoped<IWorkoutRepository, WorkoutRepository>();
            builder.Services.AddSingleton<App>();
            builder.Services.AddTransient<WorkoutListViewModel>();
            builder.Services.AddTransient<MainPage>();

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
