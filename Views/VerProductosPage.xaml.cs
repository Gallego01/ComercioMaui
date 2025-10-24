using ComercioMaui.Models;

namespace ComercioMaui.Views
{
    public partial class VerProductosPage : ContentPage
    {
        private readonly ProductoRepository _productoRepo;

        public VerProductosPage(ProductoRepository productoRepo)
        {
            InitializeComponent();
            _productoRepo = productoRepo;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            CargarProductos();
        }

        private void CargarProductos()
        {
            List<Producto> productos = _productoRepo.GetAllProductos();

            if (productos != null && productos.Count > 0)
            {
                ProductosCollection.ItemsSource = productos;
            }
            else
            {
                DisplayAlert("Sin productos", "No hay productos cargados aún.", "OK");
            }
        }

        private async void OnAgregarProductoClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync(nameof(AgregarProductoPage));
        }
    }
}
