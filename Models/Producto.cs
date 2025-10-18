using SQLite;
using System.ComponentModel.DataAnnotations;

namespace ComercioMaui.Models
{
    [Table("Producto")]
    public class Producto
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "La categoria es obligatoria")]
        public string Categoria { get; set; }

        [Required(ErrorMessage = "El precio es obligatorio")]
        public float Precio { get; set; }

        [Required(ErrorMessage = "El stock es obligatorio")]
        public int Stock { get; set; }

        [Required(ErrorMessage = "El stock minimo es obligatorio")]
        public int StockMinimo { get; set; }
    }
}
