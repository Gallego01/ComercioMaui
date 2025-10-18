using ComercioMaui.Models;
using ComercioMaui.Repository;
using ComercioMaui.Views;
using Microsoft.Maui.Controls;

namespace ComercioMaui
{
    public partial class App : Application
    {
        // Propiedad estática para almacenar el usuario autenticado (soluciona error anterior)
        public static Persona? CurrentUser { get; set; }

        private readonly IServiceProvider _serviceProvider;

        // Constructor para inyección de dependencias
        public App(PersonaRepository personaRepository, RolRepository rolRepository, IServiceProvider serviceProvider)
        {
            InitializeComponent();
            _serviceProvider = serviceProvider;

            // La primera página que ve el usuario siempre es la de Login.
            // Usamos NavigationPage para que la navegación a RegisterPage funcione correctamente.
            MainPage = new NavigationPage(_serviceProvider.GetRequiredService<LoginPage>());
        }

        // Constructor sin argumentos requerido por MAUI, aunque el de DI es el que se usa si está configurado.
        public App()
        {
            InitializeComponent();
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            // Nota: Aquí se inicia la MainPage si se usa AppShell, pero como estamos usando
            // una NavigationPage personalizada arriba, este código no se ejecutará por defecto.
            return base.CreateWindow(activationState);
        }
    }
}
