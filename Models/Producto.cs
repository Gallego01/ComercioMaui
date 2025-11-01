using System.ComponentModel.DataAnnotations;
using SQLite;

namespace ComercioMaui.Models
{
    [Table("Producto")]
    public class Producto
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [Required]
        public string Nombre { get; set; }

        [Indexed]
        [Required]
        public int CategoriaId { get; set; }

        [Required]
        public float Precio { get; set; }

        [Required]
        public int Stock { get; set; }

        [Required]
        public int StockMinimo { get; set; }

        public bool IsDeleted { get; set; } = false;

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        public bool IsFavorito { get; set; } = false;

        [Ignore]
        public string CategoriaNombre { get; set; }
    }
}
