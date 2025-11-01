namespace ComercioMaui.Views
{
    public partial class AgregarCategoriaPage : ContentPage
    {
        private readonly CategoriaRepository _categoriaRepo;

        public AgregarCategoriaPage(CategoriaRepository categoriaRepo)
        {
            InitializeComponent();
            _categoriaRepo = categoriaRepo;
        }

        private async void OnGuardarClicked(object sender, EventArgs e)
        {
            string nombre = NombreCategoriaEntry.Text?.Trim();

            if (string.IsNullOrWhiteSpace(nombre))
            {
                StatusLabel.TextColor = Colors.Red;
                StatusLabel.Text = "El nombre de la categoría no puede estar vacío.";
                return;
            }

            var nuevaCategoria = new Models.Categoria { Nombre = nombre };

            bool success = _categoriaRepo.AddCategoria(nuevaCategoria);

            StatusLabel.TextColor = success ? Colors.Green : Colors.Red;
            StatusLabel.Text = _categoriaRepo.StatusMessage;

            if (success)
            {
                NombreCategoriaEntry.Text = string.Empty;
                await DisplayAlert("Éxito", "Categoría guardada.", "OK");
                await Shell.Current.GoToAsync("..");
            }
        }
    }
}