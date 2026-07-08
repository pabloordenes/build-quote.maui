using BuildQuote.Domain.Repositories;
using BuildQuote.Infrastructure.Persistence;
using BuildQuote.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BuildQuote.App;

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

        // 1. Configuración de SQLite
        string dbPath = Path.Combine(FileSystem.AppDataDirectory, "buildquote.db3");

        builder.Services.AddDbContext<AppDbContext>(options =>
            options.UseSqlite($"Filename={dbPath}"));

        // 2. Inyección de Dependencias
        builder.Services.AddTransient<IMaterialRepository, SqliteMaterialRepository>();
        builder.Services.AddTransient<IQuoteRepository, SqliteQuoteRepository>();
        builder.Services.AddTransient<ViewModels.MaterialViewModel>();
        builder.Services.AddTransient<Views.MaterialPage>();
        builder.Services.AddTransient<ViewModels.CreateQuoteViewModel>();
        builder.Services.AddTransient<Views.CreateQuotePage>();

#if DEBUG
        builder.Logging.AddDebug();
#endif

        var app = builder.Build();

        // 3. Inicializacion de la bd
        using (var scope = app.Services.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            // Garantiza que la carpeta de Windows exista antes de que SQLite opere
            Directory.CreateDirectory(FileSystem.AppDataDirectory);

            // Crea la base de datos y las tablas de forma segura
            dbContext.Database.EnsureCreated();
        }

        return app;
    }
}