using ComercioMaui.Models;
using SQLite;
using System.ComponentModel.DataAnnotations;

namespace ComercioMaui.Repository
{
    public class PersonaRepository
    {
        private readonly string dbPath;
        private SQLiteConnection connection;
        private readonly RolRepository _rolRepo;

        public string StatusMessage { get; private set; }

        public PersonaRepository(string dbPath, RolRepository rolRepo)
        {
            _rolRepo = rolRepo;
            this.dbPath = dbPath;
            SQLitePCL.Batteries_V2.Init();
            Init();
        }

        private void Init()
        {
            if (connection != null)
                return;

            connection = new SQLiteConnection(dbPath);
            connection.CreateTable<Persona>();
        }

        public bool AddPersona(Persona persona)
        {
            try
            {
                var context = new ValidationContext(persona, null, null);
                var results = new List<ValidationResult>();

                if (!Validator.TryValidateObject(persona, context, results, true))
                {
                    StatusMessage = string.Join(Environment.NewLine, results.ConvertAll(r => r.ErrorMessage));
                    return false;
                }

                connection.Insert(persona);
                StatusMessage = "Persona agregada exitosamente.";
                return true;
            }
            catch (Exception ex)
            {
                StatusMessage = $"Error al agregar persona: {ex.Message}";
                return false;
            }
        }

        public List<Persona> GetAllPersonas()
        {
            try
            {
                return connection.Table<Persona>().ToList();
            }
            catch (Exception ex)
            {
                StatusMessage = $"Error al obtener personas: {ex.Message}";
                return new List<Persona>();
            }
        }

        public Persona? GetPersonaById(int id)
        {
            try
            {
                var persona = connection.Find<Persona>(id);
                if (persona == null)
                    StatusMessage = "No se encontro la persona.";
                return persona;
            }
            catch (Exception ex)
            {
                StatusMessage = $"Error al obtener persona: {ex.Message}";
                return null;
            }
        }

        public bool UpdatePersona(Persona persona)
        {
            try
            {
                var context = new ValidationContext(persona, null, null);
                var results = new List<ValidationResult>();

                if (!Validator.TryValidateObject(persona, context, results, true))
                {
                    StatusMessage = string.Join(Environment.NewLine, results.ConvertAll(r => r.ErrorMessage));
                    return false;
                }

                connection.Update(persona);
                StatusMessage = "Persona actualizada exitosamente.";
                return true;
            }
            catch (Exception ex)
            {
                StatusMessage = $"Error al actualizar persona: {ex.Message}";
                return false;
            }
        }

        public bool DeletePersona(int id)
        {
            try
            {
                var persona = connection.Find<Persona>(id);
                if (persona != null)
                {
                    connection.Delete(persona);
                    StatusMessage = "Persona eliminada exitosamente.";
                    return true;
                }

                StatusMessage = "No se encontro la persona.";
                return false;
            }
            catch (Exception ex)
            {
                StatusMessage = $"Error al eliminar persona: {ex.Message}";
                return false;
            }
        }

        public Persona? VerificarPersona(string usuario, string contrasena)
        {
            try
            {
                var persona = connection.Table<Persona>()
                    .FirstOrDefault(p => p.Usuario == usuario && p.Contrasena == contrasena);

                StatusMessage = persona != null ? "Inicio de sesion exitosa." : "Usuario o contraseña incorrectos.";
                return persona;
            }
            catch (Exception ex)
            {
                StatusMessage = $"Error de autenticación: {ex.Message}";
                return null;
            }
        }

        public bool RegistrarPersona(Persona persona)
        {
            try
            {
                if (connection.Table<Persona>().Any(p => p.Usuario == persona.Usuario))
                {
                    StatusMessage = "El nombre de usuario ya esta en uso.";
                    return false;
                }

                persona.RolId ??= 1;

                connection.Insert(persona);
                StatusMessage = "Usuario registrado exitosamente.";
                return true;
            }
            catch (Exception ex)
            {
                StatusMessage = $"Error al registrar persona: {ex.Message}";
                return false;
            }
        }
    }
}
