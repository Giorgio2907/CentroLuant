namespace CentroLuant.Models
{
    public class Cita
    {
        public int ID_Cita { get; set; }
        public DateOnly Fecha { get; set; }
        public TimeOnly Hora { get; set; }
        public string Estado { get; set; } = "Programada";
        public string DNI_Paciente { get; set; } = string.Empty;
        public int ID_Especialista { get; set; }

        // Para mostrar en vistas
        public string? NombrePaciente { get; set; }
        public string? NombreEspecialista { get; set; }
    }
}