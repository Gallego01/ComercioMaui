using ComercioMaui.Models;
using ComercioMaui;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ComercioMaui.Views
{
    public partial class AgregarProductoPage : ContentPage
    {
        private readonly ProductoRepository _productoRepo;
        private readonly CategoriaRepository _categoriaRepo;

        private List<Categoria> _todasLasCategorias;

        private Categoria _categoriaSeleccionada;

        public AgregarProductoPage(ProductoRepository productoRepo, CategoriaRepository categoriaRepo)
        {
            InitializeComponent();
            _productoRepo = productoRepo;
            _categoriaRepo = categoriaRepo;
        }

        private void LoadCategorias()
        {
            _todasLasCategorias = _categoriaRepo.GetAllCategorias(false);

            CategoriasCollectionView.ItemsSource = _todasLasCategorias;
        }

        private void OnCategoriaSearchBarTextChanged(object sender, TextChangedEventArgs e)
        {
            string searchText = e.NewTextValue.ToLowerInvariant();

            if (string.IsNullOrWhiteSpace(searchText))
            {
                CategoriasCollectionView.ItemsSource = null;
                CategoriasCollectionView.IsVisible = false;
            }
            else
            {
                var filteredList = _todasLasCategorias
                    .Where(c => c.Nombre.ToLowerInvariant().Contains(searchText))
                    .ToList();

                CategoriasCollectionView.ItemsSource = filteredList;
                CategoriasCollectionView.IsVisible = filteredList.Any();
            }
        }

        private void OnCategoriaSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.CurrentSelection.FirstOrDefault() is Categoria selectedCategory)
            {
                _categoriaSeleccionada = selectedCategory;
                CategoriaSeleccionadaLabel.Text = $"Categoría seleccionada: {selectedCategory.Nombre}";
            }
        }

        private async void OnGuardarProductoClicked(object sender, EventArgs e)
        {
            StatusLabel.Text = string.Empty;
            StatusLabel.TextColor = Color.FromRgb(255, 0, 0);

            if (_categoriaSeleccionada == null)
            {
                StatusLabel.Text = "Debe seleccionar una categoría de la lista.";
                await DisplayAlert("Advertencia", StatusLabel.Text, "Entendido");
                return;
            }

            if (!float.TryParse(PrecioEntry.Text, out float precio) || precio < 0)
            {
                StatusLabel.Text = "El Precio debe ser un número válido (ej. 150.99) y no puede ser negativo.";
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

            var nuevoProducto = new Producto
            {
                Nombre = NombreEntry.Text,

                CategoriaId = _categoriaSeleccionada.Id,

                Precio = precio, 
                Stock = stock,
                StockMinimo = stockMinimo 
            };

            bool success = _productoRepo.AddProducto(nuevoProducto);

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

            _todasLasCategorias = _categoriaRepo.GetAllCategorias(false);

            NombreEntry.Text = string.Empty;
            CategoriaSearchBar.Text = string.Empty;
            _categoriaSeleccionada = null;
            CategoriaSeleccionadaLabel.Text = "Categoría seleccionada: Ninguna";

            CategoriasCollectionView.IsVisible = false;

            PrecioEntry.Text = string.Empty;
            StockEntry.Text = string.Empty;
            StockMinimoEntry.Text = string.Empty;
            StatusLabel.Text = string.Empty;
        }
    }
}