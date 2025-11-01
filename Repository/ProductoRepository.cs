﻿using System.ComponentModel.DataAnnotations;
using ComercioMaui.Models;
using SQLite;
namespace ComercioMaui
{
    public class ProductoRepository
    {
        private readonly string dbPath;
        private SQLiteConnection connection;
        private readonly CategoriaRepository categoriaRepo;

        public string StatusMessage { get; private set; }

        public ProductoRepository(string dbPath, CategoriaRepository categoriaRepo)
        {
            this.dbPath = dbPath;
            this.categoriaRepo = categoriaRepo;
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

                if (!Validator.TryValidateObject(producto, context, results, true))
                {
                    StatusMessage = string.Join(Environment.NewLine, results.ConvertAll(r => r.ErrorMessage));
                    return false;
                }

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

        public Producto GetProductoById(int id)
        {
            try
            {
                var producto = connection.Find<Producto>(id);
                if (producto == null)
                {
                    StatusMessage = $"No se encontró producto con ID {id}.";
                }
                return producto;
            }
            catch (Exception ex)
            {
                StatusMessage = $"Error al obtener producto: {ex.Message}";
                return null;
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
