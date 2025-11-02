using ComercioMaui.Models;
using ComercioMaui.Repository;

namespace ComercioMaui.Views
{
    public partial class LoginPage : ContentPage
    {
        private readonly PersonaRepository _personaRepo;
        private readonly IServiceProvider _serviceProvider;

        public LoginPage(PersonaRepository personaRepo, IServiceProvider serviceProvider)
        {
            InitializeComponent();
            _personaRepo = personaRepo;
            _serviceProvider = serviceProvider;
        }

        private async void OnLoginClicked(object sender, EventArgs e)
        {
            string usuario = UsuarioEntry.Text;
            string password = ContrasenaEntry.Text;

            StatusLabel.Text = string.Empty;
            StatusLabel.TextColor = Color.FromRgb(255, 0, 0);

            Persona? authenticatedPersona = _personaRepo.VerificarPersona(usuario, password);

            if (authenticatedPersona != null)
            {
                StatusLabel.TextColor = Color.FromHex("#28a745");
                StatusLabel.Text = "Inicio de sesión exitoso.";

                App.CurrentUser = authenticatedPersona;

                var mainPage = _serviceProvider.GetRequiredService<MainPage>();

                Application.Current.MainPage = new AppShell();

                await Shell.Current.GoToAsync("//MainPage");
            }
            else
            {
                StatusLabel.Text = _personaRepo.StatusMessage;
            }
        }

        private async void OnGoToRegisterClicked(object sender, EventArgs e)
        {
            var registerPage = _serviceProvider.GetRequiredService<RegisterPage>();
            await Navigation.PushAsync(registerPage);
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            UsuarioEntry.Text = string.Empty;
            ContrasenaEntry.Text = string.Empty;
            StatusLabel.Text = string.Empty;
            App.CurrentUser = null;
        }
    }
}
