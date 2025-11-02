using System.Collections.ObjectModel;

namespace ComercioMaui.Views
{
    public partial class AgregarCategoriaPage : ContentPage
    {
        private readonly CategoriaRepository _categoriaRepo;
        private ObservableCollection<Models.Categoria> categorias;

        public AgregarCategoriaPage(CategoriaRepository categoriaRepo)
        {
            InitializeComponent();
            _categoriaRepo = categoriaRepo;

            categorias = new ObservableCollection<Models.Categoria>(_categoriaRepo.GetAllCategorias());
            CategoriasCollectionView.ItemsSource = categorias;
        }

        private async void OnGuardarClicked(object sender, EventArgs e)
        {
            string nombre = NombreCategoriaEntry.Text?.Trim();

            if (string.IsNullOrWhiteSpace(nombre))
            {
                StatusLabel.TextColor = Colors.Red;
                StatusLabel.Text = "El nombre de la categor�a no puede estar vac�o.";
                return;
            }

            var nuevaCategoria = new Models.Categoria { Nombre = nombre };

            bool success = _categoriaRepo.AddCategoria(nuevaCategoria);

            StatusLabel.TextColor = success ? Colors.Green : Colors.Red;
            StatusLabel.Text = _categoriaRepo.StatusMessage;

            if (success)
            {
                NombreCategoriaEntry.Text = string.Empty;

                categorias.Add(nuevaCategoria);

                await DisplayAlert("�xito", "Categor�a guardada.", "OK");
            }
        }

        private void RefrescarCategorias()
        {
            categorias.Clear();

            var todas = _categoriaRepo.GetAllCategorias(incluirEliminadas: true);

            foreach (var c in todas)
                categorias.Add(c);
        }


        private async void OnToggleHabilitadoClicked(object sender, EventArgs e)
        {
            var button = sender as Button;
            var categoria = button?.CommandParameter as Models.Categoria;
            if (categoria == null) return;

            categoria.IsDeleted = !categoria.IsDeleted;
            _categoriaRepo.UpdateCategoria(categoria);

            StatusLabel.TextColor = Colors.Green;
            StatusLabel.Text = categoria.IsDeleted
                ? $"Categor�a '{categoria.Nombre}' deshabilitada."
                : $"Categor�a '{categoria.Nombre}' habilitada.";

            RefrescarCategorias();
        }

        private async void OnBorrarPermanentementeClicked(object sender, EventArgs e)
        {
            var button = sender as Button;
            var categoria = button?.CommandParameter as Models.Categoria;
            if (categoria == null) return;

            bool confirm = await DisplayAlert("Confirmar eliminaci�n",
                $"�Desea eliminar permanentemente la categor�a '{categoria.Nombre}'?",
                "S�", "No");

            if (!confirm) return;

            _categoriaRepo.DeleteCategoriaPermanentemente(categoria.Id);
            categorias.Remove(categoria);

            StatusLabel.TextColor = Colors.Red;
            StatusLabel.Text = $"Categor�a '{categoria.Nombre}' eliminada permanentemente.";
        }

    }
}
