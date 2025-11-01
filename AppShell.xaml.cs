// Archivo: AppShell.xaml.cs

using ComercioMaui.Views;

namespace ComercioMaui
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            // 1. Rutas de Autenticación (ya definidas en XAML, pero se pueden registrar aquí si se necesita)
            // Routing.RegisterRoute(nameof(LoginPage), typeof(LoginPage)); 
            // Routing.RegisterRoute(nameof(RegisterPage), typeof(RegisterPage)); 

            // 2. Ruta de la nueva funcionalidad de Producto (MUY IMPORTANTE)
            Routing.RegisterRoute(nameof(AgregarProductoPage), typeof(AgregarProductoPage));
            Routing.RegisterRoute(nameof(VerProductosPage), typeof(VerProductosPage));
            Routing.RegisterRoute(nameof(AgregarCategoriaPage), typeof(AgregarCategoriaPage));

            // Si tienes una página de listado de productos, regístrala también
            // Routing.RegisterRoute(nameof(ListarProductosPage), typeof(ListarProductosPage)); 
        }
    }
}