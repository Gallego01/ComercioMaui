using ComercioMaui.Models;
using ComercioMaui;
using System;

namespace ComercioMaui.Views
{
    public partial class AgregarProductoPage : ContentPage
    {
        private readonly ProductoRepository _productoRepo;

        // Constructor que recibe el repositorio a través de Inyección de Dependencias (DI)
        // Esto asume que ProductoRepository ha sido registrado en MauiProgram.cs
        public AgregarProductoPage(ProductoRepository productoRepo)
        {
            InitializeComponent();
            _productoRepo = productoRepo;
        }

        private async void OnGuardarProductoClicked(object sender, EventArgs e)
        {
            StatusLabel.Text = string.Empty;
            StatusLabel.TextColor = Color.FromRgb(255, 0, 0);

            var nuevoProducto = new Producto
            {
                Nombre = NombreEntry.Text,
                Categoria = CategoriaEntry.Text,

                // Conversión segura de string a float y int
                Precio = float.TryParse(PrecioEntry.Text, out float precio) ? precio : 0,
                Stock = int.TryParse(StockEntry.Text, out int stock) ? stock : 0,
                StockMinimo = int.TryParse(StockMinimoEntry.Text, out int stockMinimo) ? stockMinimo : 0
            };

            // 2. Llamar al repositorio para guardar y validar
            bool success = _productoRepo.AddProducto(nuevoProducto);

            // 3. Manejar el resultado de la operación
            if (success)
            {
                StatusLabel.TextColor = Color.FromHex("#28a745");
                StatusLabel.Text = _productoRepo.StatusMessage;

                await DisplayAlert("Éxito", _productoRepo.StatusMessage, "OK");

                await Shell.Current.GoToAsync("..");
            }
            else
            {
                StatusLabel.Text = _productoRepo.StatusMessage ?? "Error desconocido al guardar el producto.";
                await DisplayAlert("Error", StatusLabel.Text, "Entendido");
            }
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            NombreEntry.Text = string.Empty;
            CategoriaEntry.Text = string.Empty;
            PrecioEntry.Text = string.Empty;
            StockEntry.Text = string.Empty;
            StockMinimoEntry.Text = string.Empty;
            StatusLabel.Text = string.Empty;
        }
    }
}