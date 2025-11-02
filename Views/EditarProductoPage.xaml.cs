using ComercioMaui.Models;
using Microsoft.Maui.Controls;
using System.Linq;
using System;

namespace ComercioMaui.Views
{
    [QueryProperty(nameof(ProductoId), "ProductoId")]
    public partial class EditarProductoPage : ContentPage
    {
        private readonly ProductoRepository _productoRepo;
        private readonly CategoriaRepository _categoriaRepo;
        private Producto _producto;
        private Categoria _categoriaSeleccionada;

        public int ProductoId { get; set; }

        public EditarProductoPage(ProductoRepository productoRepo, CategoriaRepository categoriaRepo)
        {
            InitializeComponent();
            _productoRepo = productoRepo;
            _categoriaRepo = categoriaRepo;

            CategoriaPicker.SelectedIndexChanged += (s, e) =>
            {
                _categoriaSeleccionada = CategoriaPicker.SelectedItem as Categoria;
            };
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (_producto == null && ProductoId != 0)
            {
                _producto = _productoRepo.GetAllProductos().FirstOrDefault(p => p.Id == ProductoId);
                if (_producto != null)
                    LlenarCampos(_producto);
            }
        }

        private void LlenarCampos(Producto producto)
        {
            NombreEntry.Text = producto.Nombre;
            PrecioEntry.Text = producto.Precio.ToString();
            StockEntry.Text = producto.Stock.ToString();
            StockMinimoEntry.Text = producto.StockMinimo.ToString();

            var categorias = _categoriaRepo.GetAllCategorias();
            CategoriaPicker.ItemsSource = categorias;
            CategoriaPicker.ItemDisplayBinding = new Binding("Nombre");

            _categoriaSeleccionada = categorias.FirstOrDefault(c => c.Id == producto.CategoriaId);
            CategoriaPicker.SelectedItem = _categoriaSeleccionada;
        }

        private async void OnGuardarClicked(object sender, EventArgs e)
        {
            StatusLabel.Text = string.Empty;
            StatusLabel.TextColor = Colors.Red;

            if (_categoriaSeleccionada == null)
            {
                StatusLabel.Text = "Debe seleccionar una categoría de la lista.";
                await DisplayAlert("Advertencia", StatusLabel.Text, "Entendido");
                return;
            }

            if (!float.TryParse(PrecioEntry.Text, out float precio) || precio < 0)
            {
                StatusLabel.Text = "El Precio debe ser un número válido y no puede ser negativo.";
                await DisplayAlert("Error de Formato", StatusLabel.Text, "Aceptar");
                return;
            }

            if (!int.TryParse(StockEntry.Text, out int stock) || stock < 0)
            {
                StatusLabel.Text = "El Stock Actual debe ser un número entero y no puede ser negativo.";
                await DisplayAlert("Error de Formato", StatusLabel.Text, "Aceptar");
                return;
            }

            if (!int.TryParse(StockMinimoEntry.Text, out int stockMinimo) || stockMinimo < 0)
            {
                StatusLabel.Text = "El Stock Mínimo debe ser un número entero y no puede ser negativo.";
                await DisplayAlert("Error de Formato", StatusLabel.Text, "Aceptar");
                return;
            }

            if (stockMinimo > stock)
            {
                StatusLabel.Text = "El Stock Mínimo no puede ser mayor que el Stock Actual.";
                await DisplayAlert("Error de Lógica", StatusLabel.Text, "Aceptar");
                return;
            }

            try
            {
                _producto.Nombre = NombreEntry.Text;
                _producto.Precio = precio;
                _producto.Stock = stock;
                _producto.StockMinimo = stockMinimo;
                _producto.CategoriaId = _categoriaSeleccionada.Id;

                _productoRepo.UpdateProducto(_producto);

                StatusLabel.TextColor = Colors.Green;
                StatusLabel.Text = "Producto actualizado correctamente.";
                await DisplayAlert("Éxito", StatusLabel.Text, "OK");

                await Shell.Current.GoToAsync("..");
            }
            catch (Exception ex)
            {
                StatusLabel.Text = $"Error al guardar: {ex.Message}";
                await DisplayAlert("Error", StatusLabel.Text, "OK");
            }
        }
    }
}
