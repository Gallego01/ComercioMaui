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

            string nombre = NombreEntry.Text?.Trim();
            string apellido = ApellidoEntry.Text?.Trim();
            string dni = DniEntry.Text?.Trim();
            DateTime fechaNacimiento = FechaNacimientoPicker.Date;
            string direccion = DireccionEntry.Text?.Trim();
            string telefono = TelefonoEntry.Text?.Trim();
            string email = EmailEntry.Text?.Trim();
            string usuario = UsuarioEntry.Text?.Trim();
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

            if (int.TryParse(nombre, out _) || int.TryParse(apellido, out _))
            {
                StatusLabel.Text = "El nombre y apellido no pueden ser números.";
                return;
            }

            if (!email.Contains("@"))
            {
                StatusLabel.Text = "El email debe contener '@'.";
                return;
            }

            if (!long.TryParse(dni, out _))
            {
                StatusLabel.Text = "El DNI debe contener solo números.";
                return;
            }

            if (!long.TryParse(telefono, out _))
            {
                StatusLabel.Text = "El teléfono debe contener solo números.";
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
