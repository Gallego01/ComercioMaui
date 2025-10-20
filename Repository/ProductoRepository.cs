using System.ComponentModel.DataAnnotations;
using ComercioMaui.Models;
using SQLite;
namespace ComercioMaui
{
    public class ProductoRepository
    {
        private readonly string dbPath;
        private SQLiteConnection connection;

        public string StatusMessage { get; private set; }

        public ProductoRepository(string dbPath)
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
            connection.CreateTable<Producto>();
        }

        public bool AddProducto(Producto producto)
        {
            try
            {
                var context = new ValidationContext(producto, null, null);
                var results = new List<ValidationResult>();

                // 1. Validar todos los campos usando las DataAnnotations ([Required])
                if (!Validator.TryValidateObject(producto, context, results, true))
                {
                    // Si la validación falla, StatusMessage contendrá todos los errores
                    StatusMessage = string.Join(Environment.NewLine, results.ConvertAll(r => r.ErrorMessage));
                    return false;
                }

                // 2. Insertar en la base de datos si la validación es exitosa
                var result = connection.Insert(producto);

                if (result > 0)
                {
                    StatusMessage = "Producto agregado exitosamente.";
                    return true;
                }

                StatusMessage = "No se pudo agregar el producto.";
                return false;
            }
            catch (Exception ex)
            {
                StatusMessage = $"Error al agregar producto: {ex.Message}";
                return false;
            }
        }

        public List<Producto> GetAllProductos()
        {
            try
            {
                return connection.Table<Producto>().ToList();
            }
            catch (Exception ex)
            {
                StatusMessage = $"Error al obtener productos: {ex.Message}";
                return new List<Producto>();
            }
        }

        public void DeleteProducto(int id)
        {
            try
            {
                var producto = connection.Find<Producto>(id);
                if (producto != null)
                    connection.Delete(producto);
            }
            catch (Exception ex)
            {
                StatusMessage = $"Error al eliminar: {ex.Message}";
            }
        }

        public void UpdateProducto(Producto producto)
        {
            try
            {
                connection.Update(producto);
            }
            catch (Exception ex)
            {
                StatusMessage = $"Error al actualizar: {ex.Message}";
            }
        }
    }
}
