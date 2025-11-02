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

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (App.CurrentUser != null)
            {
                UsuariosRolesLayout.IsVisible = App.CurrentUser.RolId == 2 || App.CurrentUser.RolId == 3;
            }
        }

        // ⭐ IMPLEMENTACIÓN DEL MANEJADOR DE EVENTOS ⭐
        private async void OnGoToVerProductosClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync(nameof(VerProductosPage));
        }

        private async void OnGoToGestionUsuarioClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync(nameof(GestionUsuarioPage));
        }

        private async void OnLogoutClicked(object sender, EventArgs e)
        {
            App.CurrentUser = null;

            await Shell.Current.GoToAsync("//LoginPage");
        }
    }
}