using System.ComponentModel.DataAnnotations;
using SQLite;

namespace ComercioMaui.Models
{
    [Table("Categoria")]
    public class Categoria
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [Required]
        [Unique]
        public string Nombre { get; set; }

        public bool IsDeleted { get; set; } = false;

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime UpdatedAt { get; set; } = DateTime.Now;
    }
}
