using SQLite;
using System;
using System.ComponentModel.DataAnnotations;

namespace ComercioMaui.Models
{
    [Table("Persona")]
    public class Persona
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [Required]
        public string Nombre { get; set; }

        [Required]
        public string Apellido { get; set; }

        [Required]
        public string Dni { get; set; }

        [Required]
        public DateTime FechaNacimiento { get; set; }

        [Required]
        public string Direccion { get; set; }

        [Required]
        public string Telefono { get; set; }

        [Required]
        [EmailAddress(ErrorMessage = "El email no tiene un formato valido")]
        public string Email { get; set; }

        [Required]
        public string Usuario { get; set; }

        [Required]
        public string Contrasena { get; set; }

        public int? RolId { get; set; }
    }
}
