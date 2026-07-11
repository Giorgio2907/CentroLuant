using System.ComponentModel.DataAnnotations;

namespace CentroLuant.Models
{
    public class Paciente
    {
        public string DNI { get; set; } = string.Empty;
        public string Nombres { get; set; } = string.Empty;
        public string Apellidos { get; set; } = string.Empty;
        public DateOnly? FechaNacimiento { get; set; }
        public string? Direccion { get; set; }

        [RegularExpression(@"^\d{9}$", ErrorMessage = "El teléfono debe tener exactamente 9 dígitos.")]
        public string? Telefono { get; set; }

        [RegularExpression(
            @"^[^@\s]+@(gmail\.com|hotmail\.com|unmsm\.edu\.pe)$",
            ErrorMessage = "El correo debe ser de dominio @gmail.com, @hotmail.com o @unmsm.edu.pe")]
        public string? CorreoElectronico { get; set; }

        public string? Genero { get; set; }

        public string NombreCompleto => $"{Nombres} {Apellidos}";
    }
}