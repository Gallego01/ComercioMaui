using ComercioMaui.Models;
using ComercioMaui.Repository;

namespace ComercioMaui.Views;

public partial class AsignarRolPage : ContentPage
{
    private readonly PersonaRepository _personaRepo;
    private readonly RolRepository _rolRepo;
    private List<Persona> _personas;
    private Persona _personaSeleccionada;

    public AsignarRolPage(PersonaRepository personaRepo, RolRepository rolRepo)
    {
        InitializeComponent();

        _personaRepo = personaRepo;
        _rolRepo = rolRepo;

        _personas = _personaRepo.GetAllPersonas();

        RolPicker.ItemsSource = _rolRepo.GetAllRoles().Select(r => r.Nombre).ToList();
    }

    private void DniEntry_TextChanged(object sender, TextChangedEventArgs e)
    {
        string filtro = e.NewTextValue.Trim();

        if (string.IsNullOrEmpty(filtro))
        {
            DniSuggestionsCollectionView.ItemsSource = null;
            return;
        }

        var sugerencias = _personas
            .Where(p => p.Dni.Contains(filtro))
            .ToList();

        DniSuggestionsCollectionView.ItemsSource = sugerencias;
    }

    private void DniSuggestionsCollectionView_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection.Count == 0)
            return;

        _personaSeleccionada = e.CurrentSelection[0] as Persona;

        if (_personaSeleccionada != null)
        {
            NombreLabel.Text = $"Nombre: {_personaSeleccionada.Nombre}";
            ApellidoLabel.Text = $"Apellido: {_personaSeleccionada.Apellido}";
            TelefonoLabel.Text = $"Teléfono: {_personaSeleccionada.Telefono}";
            EmailLabel.Text = $"Email: {_personaSeleccionada.Email}";
            DireccionLabel.Text = $"Dirección: {_personaSeleccionada.Direccion}";

            if (_personaSeleccionada.RolId.HasValue)
            {
                var rol = _rolRepo.GetRolById(_personaSeleccionada.RolId.Value);
                RolLabel.Text = $"Rol actual: {rol?.Nombre ?? "Sin rol"}";
            }
            else
            {
                RolLabel.Text = "Rol actual: Sin rol";
            }

            DniEntry.Text = _personaSeleccionada.Dni;

            DniSuggestionsCollectionView.ItemsSource = null;
        }
    }

    private void OnAsignarRolClicked(object sender, EventArgs e)
    {
        if (_personaSeleccionada == null)
        {
            StatusLabel.Text = "Seleccione primero una persona.";
            return;
        }

        if (RolPicker.SelectedIndex == -1)
        {
            StatusLabel.Text = "Seleccione un rol para asignar.";
            return;
        }

        var rolNombre = RolPicker.SelectedItem.ToString();
        var rol = _rolRepo.GetRolByName(rolNombre);

        if (rol == null)
        {
            StatusLabel.Text = "Rol no válido.";
            return;
        }

        _personaSeleccionada.RolId = rol.Id;

        if (_personaRepo.UpdatePersona(_personaSeleccionada))
        {
            StatusLabel.TextColor = Colors.Green;
            StatusLabel.Text = $"Rol '{rol.Nombre}' asignado correctamente a {_personaSeleccionada.Nombre}.";
            RolLabel.Text = $"Rol actual: {rol.Nombre}";
        }
        else
        {
            StatusLabel.TextColor = Colors.Red;
            StatusLabel.Text = _personaRepo.StatusMessage;
        }
    }
}
