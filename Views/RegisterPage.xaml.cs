using ComercioMaui.Models;
using ComercioMaui.Repository;
using Microsoft.Maui.Controls;
using System;

namespace ComercioMaui.Views
{
    public partial class RegisterPage : ContentPage
    {
        private readonly PersonaRepository _personaRepository;

        public RegisterPage(PersonaRepository personaRepository)
        {
            InitializeComponent();
            _personaRepository = personaRepository;
        }

        private async void OnRegisterClicked(object sender, EventArgs e)
        {
            StatusLabel.Text = string.Empty;

            string nombre = NombreEntry.Text;
            string apellido = ApellidoEntry.Text;
            string dni = DniEntry.Text;
            DateTime fechaNacimiento = FechaNacimientoPicker.Date;
            string direccion = DireccionEntry.Text;
            string telefono = TelefonoEntry.Text;
            string email = EmailEntry.Text;
            string usuario = UsuarioEntry.Text;
            string contrasena = ContrasenaEntry.Text;
            string confirmarContrasena = ConfirmarContrasenaEntry.Text;

            if (string.IsNullOrWhiteSpace(nombre) || string.IsNullOrWhiteSpace(apellido) ||
                string.IsNullOrWhiteSpace(dni) || string.IsNullOrWhiteSpace(direccion) ||
                string.IsNullOrWhiteSpace(telefono) || string.IsNullOrWhiteSpace(email) ||
                string.IsNullOrWhiteSpace(usuario) || string.IsNullOrWhiteSpace(contrasena))
            {
                StatusLabel.Text = "Todos los campos son obligatorios.";
                return;
            }

            if (contrasena != confirmarContrasena)
            {
                StatusLabel.Text = "Las contraseñas no coinciden.";
                return;
            }

            var nuevaPersona = new Persona
            {
                Nombre = nombre,
                Apellido = apellido,
                Dni = dni,
                FechaNacimiento = fechaNacimiento,
                Direccion = direccion,
                Telefono = telefono,
                Email = email,
                Usuario = usuario,
                Contrasena = contrasena,
                RolId = null
            };

            try
            {
                bool success = _personaRepository.RegistrarPersona(nuevaPersona);

                if (success)
                {
                    await DisplayAlert("Éxito", "Registro completado. Ahora puede iniciar sesión.", "OK");

                    await Navigation.PopAsync();
                }
                else
                {
                    StatusLabel.Text = $"Error de registro: {_personaRepository.StatusMessage}";
                }
            }
            catch (Exception ex)
            {
                StatusLabel.Text = $"Error inesperado: {ex.Message}";
            }
        }
    }
}
