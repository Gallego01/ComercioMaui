using SQLite;
using System.ComponentModel.DataAnnotations;
using ComercioMaui.Models;

namespace ComercioMaui.Repository
{
    public class RolRepository
    {
        private readonly string dbPath;
        private SQLiteConnection connection;

        public string StatusMessage { get; private set; } = string.Empty;

        public RolRepository(string dbPath)
        {
            this.dbPath = dbPath;
            SQLitePCL.Batteries_V2.Init();
            Init();
        }

        private void Init()
        {
            if (connection != null)
                return;

            connection = new SQLiteConnection(dbPath);
            connection.CreateTable<Rol>();
        }

        public bool AddRol(Rol rol)
        {
            try
            {
                var context = new ValidationContext(rol, null, null);
                var results = new List<ValidationResult>();

                if (!Validator.TryValidateObject(rol, context, results, true))
                {
                    StatusMessage = string.Join(Environment.NewLine, results.ConvertAll(r => r.ErrorMessage));
                    return false;
                }

                connection.Insert(rol);
                StatusMessage = "Rol agregado exitosamente.";
                return true;
            }
            catch (Exception ex)
            {
                StatusMessage = $"Error al agregar rol: {ex.Message}";
                return false;
            }
        }

        public List<Rol> GetAllRoles()
        {
            try
            {
                return connection.Table<Rol>().ToList();
            }
            catch (Exception ex)
            {
                StatusMessage = $"Error al obtener roles: {ex.Message}";
                return new List<Rol>();
            }
        }

        public Rol? GetRolByName(string nombre)
        {
            try
            {
                return connection.Table<Rol>().FirstOrDefault(r => r.Nombre == nombre);
            }
            catch (Exception ex)
            {
                StatusMessage = $"Error al obtener rol por nombre: {ex.Message}";
                return null;
            }
        }

        public Rol? GetRolById(int id)
        {
            try
            {
                var rol = connection.Find<Rol>(id);
                if (rol == null)
                    StatusMessage = "No se encontró el rol con ese ID.";
                return rol;
            }
            catch (Exception ex)
            {
                StatusMessage = $"Error al obtener rol por ID: {ex.Message}";
                return null;
            }
        }

        public bool UpdateRol(Rol rol)
        {
            try
            {
                var context = new ValidationContext(rol, null, null);
                var results = new List<ValidationResult>();

                if (!Validator.TryValidateObject(rol, context, results, true))
                {
                    StatusMessage = string.Join(Environment.NewLine, results.ConvertAll(r => r.ErrorMessage));
                    return false;
                }

                connection.Update(rol);
                StatusMessage = "Rol actualizado exitosamente.";
                return true;
            }
            catch (Exception ex)
            {
                StatusMessage = $"Error al actualizar rol: {ex.Message}";
                return false;
            }
        }

        public bool DeleteRol(int id)
        {
            try
            {
                var rol = connection.Find<Rol>(id);
                if (rol != null)
                {
                    connection.Delete(rol);
                    StatusMessage = "Rol eliminado exitosamente.";
                    return true;
                }
                else
                {
                    StatusMessage = "No se encontro el rol.";
                    return false;
                }
            }
            catch (Exception ex)
            {
                StatusMessage = $"Error al eliminar rol: {ex.Message}";
                return false;
            }
        }
    }
}
