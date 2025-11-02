using System.Collections.ObjectModel;
using ComercioMaui.Models;
using ComercioMaui.Repository;
using Microsoft.Maui.Controls;

namespace ComercioMaui.Views
{
    public partial class AgregarRolPage : ContentPage
    {
        private readonly RolRepository _rolRepo;
        private ObservableCollection<Rol> roles;

        public AgregarRolPage(RolRepository rolRepo)
        {
            InitializeComponent();
            _rolRepo = rolRepo;

            roles = new ObservableCollection<Rol>(_rolRepo.GetAllRoles());
            RolesCollectionView.ItemsSource = roles;
        }

        private async void OnGuardarClicked(object sender, EventArgs e)
        {
            string nombre = NombreRolEntry.Text?.Trim();

            if (string.IsNullOrWhiteSpace(nombre))
            {
                StatusLabel.TextColor = Colors.Red;
                StatusLabel.Text = "El nombre del rol no puede estar vacío.";
                return;
            }

            var nuevoRol = new Rol { Nombre = nombre };

            bool success = _rolRepo.AddRol(nuevoRol);

            StatusLabel.TextColor = success ? Colors.Green : Colors.Red;
            StatusLabel.Text = _rolRepo.StatusMessage;

            if (success)
            {
                NombreRolEntry.Text = string.Empty;
                roles.Add(nuevoRol);
                await DisplayAlert("Éxito", "Rol guardado.", "OK");
            }
        }

        private async void OnBorrarRolClicked(object sender, EventArgs e)
        {
            var button = sender as Button;
            var rol = button?.CommandParameter as Rol;
            if (rol == null) return;

            bool confirm = await DisplayAlert("Confirmar eliminación",
                $"¿Desea eliminar permanentemente el rol '{rol.Nombre}'?",
                "Sí", "No");

            if (!confirm) return;

            _rolRepo.DeleteRol(rol.Id);
            roles.Remove(rol);

            StatusLabel.TextColor = Colors.Red;
            StatusLabel.Text = $"Rol '{rol.Nombre}' eliminado permanentemente.";
        }
    }
}
