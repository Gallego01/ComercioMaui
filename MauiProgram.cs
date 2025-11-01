using Microsoft.Extensions.Logging;
using ComercioMaui.Repository;
using ComercioMaui.Views;
using System;
using System.IO;
using ComercioMaui.Models;

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
            var dbPath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                "comercio.db3"
            );

            // --- REGISTRO DE REPOSITORIOS ---
            builder.Services.AddSingleton<RolRepository>(s =>
                new RolRepository(dbPath)
            );

            builder.Services.AddSingleton<PersonaRepository>(s =>
            {
                var rolRepo = s.GetRequiredService<RolRepository>();
                return new PersonaRepository(dbPath, rolRepo);
            });

            builder.Services.AddSingleton<CategoriaRepository>(s =>
                new CategoriaRepository(dbPath)
            );

            builder.Services.AddSingleton<ProductoRepository>(s =>
            {
                var categoriaRepo = s.GetRequiredService<CategoriaRepository>();
                return new ProductoRepository(dbPath, categoriaRepo);
            });

            // --- REGISTRO DE PÁGINAS / VISTAS ---
            builder.Services.AddTransient<LoginPage>();
            builder.Services.AddTransient<RegisterPage>();
            builder.Services.AddTransient<MainPage>();

            return builder.Build();
        }
    }
}
