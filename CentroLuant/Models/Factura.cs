using System.ComponentModel.DataAnnotations;

namespace CentroLuant.Models
{
    public class Factura
    {
        public int ID_Factura { get; set; }
        public string DNI_Paciente { get; set; } = string.Empty;
        public DateOnly FechaEmision { get; set; }

        [Range(0, 99999.99, ErrorMessage = "El monto no puede tener más de 5 dígitos (máximo 99999.99).")]
        public decimal MontoTotal { get; set; }

        public string? DescripcionServicios { get; set; }
        public string EstadoPago { get; set; } = "Pendiente";

        public string? NombrePaciente { get; set; }
    }
}