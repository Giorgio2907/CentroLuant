using System.ComponentModel.DataAnnotations;

namespace CentroLuant.Models
{
    public class Tratamiento
    {
        public int ID_Tratamiento { get; set; }
        public int ID_Historial { get; set; }
        public DateOnly FechaTratamiento { get; set; }
        public string? Diagnostico { get; set; }
        public string? TipoTratamiento { get; set; }
        public string? Observaciones { get; set; }

        [Range(0, 99999.99, ErrorMessage = "El costo no puede tener más de 5 dígitos (máximo 99999.99).")]
        public decimal Costo { get; set; }
    }
}