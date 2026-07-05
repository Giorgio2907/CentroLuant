namespace CentroLuant.Models
{
    public class Paciente
    {
        public string DNI { get; set; } = string.Empty;
        public string Nombres { get; set; } = string.Empty;
        public string Apellidos { get; set; } = string.Empty;
        public DateOnly? FechaNacimiento { get; set; }
        public string? Direccion { get; set; }
        public string? Telefono { get; set; }
        public string? CorreoElectronico { get; set; }
        public string? Genero { get; set; }

        public string NombreCompleto => $"{Nombres} {Apellidos}";
    }
}
