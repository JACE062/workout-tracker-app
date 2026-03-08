using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore; 
using WorkoutTracker.Data;
using WorkoutTracker.Data.Repositories;
using WorkoutTrackerApp.ViewModels;
using WorkoutTrackerApp.Views;

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
            builder.Services.AddTransient<MainPage>();
            builder.Services.AddTransient<SessionsListViewModel>();
            builder.Services.AddTransient<SessionsListView>();
            builder.Services.AddTransient<SessionViewModel>();
            builder.Services.AddTransient<SessionView>();
            builder.Services.AddTransient<CreateEditWorkoutViewModel>();
            builder.Services.AddTransient<CreateEditWorkoutView>();
            builder.Services.AddTransient<TestingUserViewModel>();
            builder.Services.AddTransient<TestingUserView>();


#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
