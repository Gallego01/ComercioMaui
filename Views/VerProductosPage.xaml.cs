using ComercioMaui.Models;
// using ComercioMaui.Repositories; // <<-- ELIMINADA O COMENTADA: CAUSA EL ERROR CS0234
using Microsoft.Maui.Controls;
using System.Collections.Generic;
using System.Linq;
using System;

namespace ComercioMaui.Views
{
    public partial class VerProductosPage : ContentPage
    {
        private readonly ProductoRepository _productoRepo;
        private readonly CategoriaRepository _categoriaRepo;
        private List<Producto> _todosLosProductos;


        public VerProductosPage(ProductoRepository productoRepo, CategoriaRepository categoriaRepo)
        {
            InitializeComponent();
            _productoRepo = productoRepo;
            _categoriaRepo = categoriaRepo;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            CargarCategoriasYProductos();

            if (App.CurrentUser != null)
            {
                ProductosButtonsLayout.IsVisible = App.CurrentUser.RolId == 2 || App.CurrentUser.RolId == 3;
            }
        }

        private void CargarCategoriasYProductos()
        {

            List<Categoria> categorias = _categoriaRepo.GetAllCategorias(false);
            Categoria todas = new Categoria { Id = 0, Nombre = "Todas las Categorías" };
            categorias.Insert(0, todas);
            CategoriaPicker.ItemsSource = categorias;
            CategoriaPicker.ItemDisplayBinding = new Binding("Nombre");
            CategoriaPicker.SelectedItem = todas;

            _todosLosProductos = _productoRepo.GetAllProductos();

            OnCategoriaFilterChanged(null, null);

            if (_todosLosProductos == null || _todosLosProductos.Count == 0)
            {
                Shell.Current.DisplayAlert("Sin productos", "No hay productos cargados aún.", "OK");
            }
        }


        private void OnCategoriaFilterChanged(object sender, EventArgs e)
        {
            if (_todosLosProductos == null) return;

            IEnumerable<Producto> productosFiltrados = _todosLosProductos;


            if (CategoriaPicker.SelectedItem is Categoria categoriaSeleccionada && categoriaSeleccionada.Id != 0)
            {
                productosFiltrados = productosFiltrados
                    .Where(p => p.CategoriaId == categoriaSeleccionada.Id);
            }


            if (FavoritosSwitch.IsToggled)
            {
                productosFiltrados = productosFiltrados
                    .Where(p => p.IsFavorito);
            }


            ProductosCollection.ItemsSource = productosFiltrados.ToList();
            ProductosCollection.SelectedItem = null;
        }

        private void OnFavoritosSwitchToggled(object sender, ToggledEventArgs e)
        {
            OnCategoriaFilterChanged(sender, e);
        }

        private void OnToggleFavoritoClicked(object sender, EventArgs e)
        {

            if (sender is ImageButton button && button.BindingContext is Producto producto)
            {
                bool nuevoEstado = !producto.IsFavorito;

                try
                {
                    _productoRepo.ToggleFavorito(producto.Id, nuevoEstado);

                    producto.IsFavorito = nuevoEstado;


                    if (FavoritosSwitch.IsToggled)
                    {
                        OnCategoriaFilterChanged(null, null);
                    }
                }
                catch (Exception ex)
                {
                    // Usar DisplayAlert es correcto aquí
                    DisplayAlert("Error", $"No se pudo actualizar el estado de favorito: {ex.Message}", "OK");
                }
            }
        }


        private async void OnAgregarProductoClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync(nameof(AgregarProductoPage));
        }

        private async void OnEditarProductoClicked(object sender, EventArgs e)
        {
            if (ProductosCollection.SelectedItem is Producto productoSeleccionado)
            {
                var parameters = new Dictionary<string, object>
                {
                    { "ProductoId", productoSeleccionado.Id }
                };

                await Shell.Current.GoToAsync(nameof(EditarProductoPage), parameters);
            }
            else
            {
                // Usar DisplayAlert es correcto aquí
                await DisplayAlert("Seleccionar producto", "Debes seleccionar un producto para editar.", "OK");
            }
        }

        private async void OnEliminarProductoClicked(object sender, EventArgs e)
        {
            if (ProductosCollection.SelectedItem is Producto productoSeleccionado)
            {
                // Usar DisplayAlert es correcto aquí
                bool confirm = await DisplayAlert(
                    "Confirmar eliminación",
                    $"¿Seguro que quieres eliminar '{productoSeleccionado.Nombre}'?",
                    "Sí",
                    "No");

                if (confirm)
                {
                    _productoRepo.DeleteProducto(productoSeleccionado.Id);

                    // Recargar la lista de todos los productos y el filtro
                    _todosLosProductos = _productoRepo.GetAllProductos();
                    OnCategoriaFilterChanged(null, null);

                    ProductosCollection.SelectedItem = null;

                    await DisplayAlert("Eliminado", "Producto eliminado correctamente.", "OK");
                }
            }
            else
            {
                await DisplayAlert("Seleccionar producto", "Debes seleccionar un producto para eliminar.", "OK");
            }
        }
    }
}
