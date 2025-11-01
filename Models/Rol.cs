using SQLite;
using System.ComponentModel.DataAnnotations;

namespace ComercioMaui.Models
{
    [Table("Rol")]
    public class Rol
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [Required]
        public string Nombre { get; set; }
    }
}
