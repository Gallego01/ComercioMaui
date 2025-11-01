using ComercioMaui.Models;
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

            ProductosCollection.ItemsSource = _todosLosProductos;

            if (_todosLosProductos == null || _todosLosProductos.Count == 0)
            {
                Shell.Current.DisplayAlert("Sin productos", "No hay productos cargados aún.", "OK");
            }
        }

        private void OnCategoriaFilterChanged(object sender, EventArgs e)
        {
            if (_todosLosProductos == null) return;

            if (CategoriaPicker.SelectedItem is Categoria categoriaSeleccionada)
            {
                if (categoriaSeleccionada.Id == 0)
                {
                    ProductosCollection.ItemsSource = _todosLosProductos;
                }
                else
                {
                    var productosFiltrados = _todosLosProductos
                        .Where(p => p.CategoriaId == categoriaSeleccionada.Id)
                        .ToList();

                    ProductosCollection.ItemsSource = productosFiltrados;
                }
            }
        }

        private async void OnAgregarProductoClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync(nameof(AgregarProductoPage));
        }
    }
}