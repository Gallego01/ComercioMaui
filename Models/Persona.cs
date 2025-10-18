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

        [Required(ErrorMessage = "El nombre es obligatorio")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "El apellido es obligatorio")]
        public string Apellido { get; set; }

        [Required(ErrorMessage = "El DNI es obligatorio")]
        public string Dni { get; set; }

        [Required(ErrorMessage = "La fecha de nacimiento es obligatoria")]
        public DateTime FechaNacimiento { get; set; }

        [Required(ErrorMessage = "La direccion es obligatoria")]
        public string Direccion { get; set; }

        [Required(ErrorMessage = "El telefono es obligatorio")]
        public string Telefono { get; set; }

        [Required(ErrorMessage = "El email es obligatorio")]
        [EmailAddress(ErrorMessage = "El email no tiene un formato valido")]
        public string Email { get; set; }

        [Required(ErrorMessage = "El usuario es obligatorio")]
        public string Usuario { get; set; }

        [Required(ErrorMessage = "La contraseña es obligatoria")]
        public string Contrasena { get; set; }

        public int? RolId { get; set; }
    }
}
