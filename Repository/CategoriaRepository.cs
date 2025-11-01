using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ComercioMaui.Models;
using SQLite;

namespace ComercioMaui
{
    public class CategoriaRepository
    {
        private readonly string dbPath;
        private SQLiteConnection connection;

        public string StatusMessage { get; private set; }

        public CategoriaRepository(string dbPath)
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
            connection.CreateTable<Categoria>();
        }

        public bool AddCategoria(Categoria categoria)
        {
            try
            {
                var context = new ValidationContext(categoria, null, null);
                var results = new List<ValidationResult>();

                if (!Validator.TryValidateObject(categoria, context, results, true))
                {
                    StatusMessage = string.Join(Environment.NewLine, results.ConvertAll(r => r.ErrorMessage));
                    return false;
                }

                var existe = connection.Table<Categoria>().FirstOrDefault(c => c.Nombre == categoria.Nombre && !c.IsDeleted);
                if (existe != null)
                {
                    StatusMessage = "Ya existe una categoría con ese nombre.";
                    return false;
                }

                var result = connection.Insert(categoria);
                if (result > 0)
                {
                    StatusMessage = "Categoría agregada exitosamente.";
                    return true;
                }

                StatusMessage = "No se pudo agregar la categoría.";
                return false;
            }
            catch (Exception ex)
            {
                StatusMessage = $"Error al agregar categoría: {ex.Message}";
                return false;
            }
        }

        public Categoria GetCategoriaById(int id)
        {
            try
            {
                var categoria = connection.Find<Categoria>(id);
                if (categoria == null)
                {
                    StatusMessage = $"No se encontró la categoría con ID {id}.";
                }
                return categoria;
            }
            catch (Exception ex)
            {
                StatusMessage = $"Error al obtener categoría: {ex.Message}";
                return null;
            }
        }

        public List<Categoria> GetAllCategorias(bool incluirEliminadas = false)
        {
            try
            {
                if (incluirEliminadas)
                    return connection.Table<Categoria>().ToList();

                return connection.Table<Categoria>().Where(c => !c.IsDeleted).ToList();
            }
            catch (Exception ex)
            {
                StatusMessage = $"Error al obtener categorías: {ex.Message}";
                return new List<Categoria>();
            }
        }

        public void UpdateCategoria(Categoria categoria)
        {
            try
            {
                categoria.UpdatedAt = DateTime.Now;
                connection.Update(categoria);
                StatusMessage = "Categoría actualizada correctamente.";
            }
            catch (Exception ex)
            {
                StatusMessage = $"Error al actualizar categoría: {ex.Message}";
            }
        }

        public void DeleteCategoria(int id)
        {
            try
            {
                var categoria = connection.Find<Categoria>(id);
                if (categoria != null)
                {
                    categoria.IsDeleted = true;
                    categoria.UpdatedAt = DateTime.Now;
                    connection.Update(categoria);
                    StatusMessage = "Categoría eliminada lógicamente.";
                }
                else
                {
                    StatusMessage = "Categoría no encontrada.";
                }
            }
            catch (Exception ex)
            {
                StatusMessage = $"Error al eliminar categoría: {ex.Message}";
            }
        }

        public void DeleteCategoriaPermanentemente(int id)
        {
            try
            {
                var categoria = connection.Find<Categoria>(id);
                if (categoria != null)
                {
                    connection.Delete(categoria);
                    StatusMessage = "Categoría eliminada permanentemente.";
                }
            }
            catch (Exception ex)
            {
                StatusMessage = $"Error al eliminar permanentemente: {ex.Message}";
            }
        }
    }
}
