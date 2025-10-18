using SQLite;
using ComercioMaui.Models;
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

        public void AddProducto(Producto producto)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(producto.Nombre))
                    throw new Exception("El nombre del producto es obligatorio.");

                var result = connection.Insert(producto);
                StatusMessage = $"{result} registro(s) agregado(s)";
            }
            catch (Exception ex)
            {
                StatusMessage = $"Error: {ex.Message}";
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
