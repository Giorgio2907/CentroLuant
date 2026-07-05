namespace CentroLuant.Models
{
    public class HistorialMedico
    {
        public int ID_Historial { get; set; }
        public string DNI_Paciente { get; set; } = string.Empty;
        public DateOnly FechaCreacion { get; set; }
        public string? ObservacionesIniciales { get; set; }

        public string? NombrePaciente { get; set; }
    }
}