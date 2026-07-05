using CentroLuant.Models;
using Dapper;

namespace CentroLuant.Repositories
{
    public class FacturaRepository
    {
        private readonly ConexionBD _conexion;

        public FacturaRepository(ConexionBD conexion)
        {
            _conexion = conexion;
        }

        public IEnumerable<Factura> ObtenerTodas()
        {
            using var db = _conexion.ObtenerConexion();
            return db.Query<Factura>(@"
                SELECT f.*, p.Nombres + ' ' + p.Apellidos AS NombrePaciente
                FROM Factura f
                LEFT JOIN Paciente p ON f.DNI_Paciente = p.DNI");
        }

        public IEnumerable<Factura> ObtenerPorPaciente(string dni)
        {
            using var db = _conexion.ObtenerConexion();
            return db.Query<Factura>(@"
                SELECT f.*, p.Nombres + ' ' + p.Apellidos AS NombrePaciente
                FROM Factura f
                LEFT JOIN Paciente p ON f.DNI_Paciente = p.DNI
                WHERE f.DNI_Paciente = @DNI", new { DNI = dni });
        }

        public Factura? ObtenerPorId(int id)
        {
            using var db = _conexion.ObtenerConexion();
            return db.QueryFirstOrDefault<Factura>(@"
                SELECT f.*, p.Nombres + ' ' + p.Apellidos AS NombrePaciente
                FROM Factura f
                LEFT JOIN Paciente p ON f.DNI_Paciente = p.DNI
                WHERE f.ID_Factura = @ID", new { ID = id });
        }

        public void Registrar(Factura factura)
        {
            using var db = _conexion.ObtenerConexion();
            db.Execute(@"
                INSERT INTO Factura (DNI_Paciente, FechaEmision, MontoTotal, DescripcionServicios, EstadoPago)
                VALUES (@DNI_Paciente, @FechaEmision, @MontoTotal, @DescripcionServicios, @EstadoPago)", factura);
        }

        public void ActualizarEstado(int id, string estado)
        {
            using var db = _conexion.ObtenerConexion();
            db.Execute("UPDATE Factura SET EstadoPago = @Estado WHERE ID_Factura = @ID",
                new { Estado = estado, ID = id });
        }
    }
}
