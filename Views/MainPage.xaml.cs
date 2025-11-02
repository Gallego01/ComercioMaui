using ComercioMaui.Models;

namespace ComercioMaui.Views
{
    // Asegúrate de que el namespace sea 'ComercioMaui.Views' para consistencia
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        // ⭐ IMPLEMENTACIÓN DEL MANEJADOR DE EVENTOS ⭐
        private async void OnGoToAgregarProductoClicked(object sender, EventArgs e)
        {
            // Navega a la ruta que registraste en AppShell.xaml.cs
            await Shell.Current.GoToAsync(nameof(AgregarProductoPage));
        }

        private async void OnGoToVerProductosClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync(nameof(VerProductosPage));
        }

        private async void OnGoToGestionUsuarioClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync(nameof(GestionUsuarioPage));
        }
    }
}