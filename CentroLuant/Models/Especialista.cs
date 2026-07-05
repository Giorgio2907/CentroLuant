namespace CentroLuant.Models
{
    public class Especialista
    {
        public int ID_Especialista { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Apellidos { get; set; } = string.Empty;
        public string? Especialidad { get; set; }

        public string NombreCompleto => $"{Nombre} {Apellidos}";
    }
}
