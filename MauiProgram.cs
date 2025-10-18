using Microsoft.Extensions.Logging;
using ComercioMaui.Repository;
using ComercioMaui.Views;
using System;
using System.IO;

namespace ComercioMaui
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            SQLitePCL.Batteries_V2.Init();

            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

#if DEBUG
            builder.Logging.AddDebug();
#endif

            // --- RUTA COMÚN DE BASE DE DATOS ---
            var dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "comercio.db3");

            // --- REGISTRO DE DEPENDENCIAS DE REPOSITORIOS ---

            // 1. RolRepository (solo dbPath)
            builder.Services.AddSingleton<RolRepository>(s => new RolRepository(dbPath));

            // 2. PersonaRepository (dbPath + RolRepository)
            builder.Services.AddSingleton<PersonaRepository>(s =>
            {
                var rolRepo = s.GetRequiredService<RolRepository>();
                return new PersonaRepository(dbPath, rolRepo);
            });

            // 3. ProductoRepository (solo dbPath)
            builder.Services.AddSingleton<ProductoRepository>(s => new ProductoRepository(dbPath));

            // --- REGISTRO DE VISTAS ---
            builder.Services.AddTransient<LoginPage>();
            builder.Services.AddTransient<RegisterPage>();
            builder.Services.AddTransient<MainPage>();

            return builder.Build();
        }
    }
}
