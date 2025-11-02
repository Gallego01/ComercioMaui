using ComercioMaui.Models;
using ComercioMaui.Repository;
using Microsoft.Maui.Controls;
using System;
using System.Collections.Generic;

namespace ComercioMaui.Views
{
    public partial class GestionUsuarioPage : ContentPage
    {
        private readonly PersonaRepository _personaRepo;
        private readonly RolRepository _rolRepo;

        public GestionUsuarioPage(PersonaRepository personaRepo, RolRepository rolRepo)
        {
            InitializeComponent();
            _personaRepo = personaRepo;
            _rolRepo = rolRepo;
        }
        private async void OnGoToAgregarCategoriaClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync(nameof(AgregarCategoriaPage));
        }

        private async void OnGoToAgregarProductoClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync(nameof(AgregarProductoPage));
        }

        private async void OnGoToAgregarRolClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync(nameof(AgregarRolPage));
        }

        private async void OnGoToAsignarRolClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync(nameof(AsignarRolPage));
        }

        /*private async void OnAgregarUsuarioClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync(nameof(RegistrarUsuarioPage));
        }

        private async void OnEditarUsuarioClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync(nameof(EditarUsuarioPage));
        }

        private async void OnEliminarUsuarioClicked(object sender, EventArgs e)
        {
            await DisplayAlert("Eliminar usuario", "Función en desarrollo.", "OK");
        }

        private async void OnVerUsuariosClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync(nameof(VerUsuariosPage));
        }*/
    }
}
