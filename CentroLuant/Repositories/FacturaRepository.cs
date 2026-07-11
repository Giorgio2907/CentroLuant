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
            var result = db.Query<dynamic>(@"
                SELECT f.*, p.Nombres + ' ' + p.Apellidos AS NombrePaciente
                FROM Factura f
                LEFT JOIN Paciente p ON f.DNI_Paciente = p.DNI");
            return result.Select(r => new Factura
            {
                ID_Factura = r.ID_Factura,
                DNI_Paciente = r.DNI_Paciente,
                FechaEmision = DateOnly.FromDateTime((DateTime)r.FechaEmision),
                MontoTotal = r.MontoTotal,
                DescripcionServicios = r.DescripcionServicios,
                EstadoPago = r.EstadoPago,
                NombrePaciente = r.NombrePaciente
            });
        }

        public IEnumerable<Factura> ObtenerPorPaciente(string dni)
        {
            using var db = _conexion.ObtenerConexion();
            var result = db.Query<dynamic>(@"
                SELECT f.*, p.Nombres + ' ' + p.Apellidos AS NombrePaciente
                FROM Factura f
                LEFT JOIN Paciente p ON f.DNI_Paciente = p.DNI
                WHERE f.DNI_Paciente = @DNI", new { DNI = dni });
            return result.Select(r => new Factura
            {
                ID_Factura = r.ID_Factura,
                DNI_Paciente = r.DNI_Paciente,
                FechaEmision = DateOnly.FromDateTime((DateTime)r.FechaEmision),
                MontoTotal = r.MontoTotal,
                DescripcionServicios = r.DescripcionServicios,
                EstadoPago = r.EstadoPago,
                NombrePaciente = r.NombrePaciente
            });
        }

        public Factura? ObtenerPorId(int id)
        {
            using var db = _conexion.ObtenerConexion();
            var r = db.QueryFirstOrDefault<dynamic>(@"
                SELECT f.*, p.Nombres + ' ' + p.Apellidos AS NombrePaciente
                FROM Factura f
                LEFT JOIN Paciente p ON f.DNI_Paciente = p.DNI
                WHERE f.ID_Factura = @ID", new { ID = id });
            if (r == null) return null;
            return new Factura
            {
                ID_Factura = r.ID_Factura,
                DNI_Paciente = r.DNI_Paciente,
                FechaEmision = DateOnly.FromDateTime((DateTime)r.FechaEmision),
                MontoTotal = r.MontoTotal,
                DescripcionServicios = r.DescripcionServicios,
                EstadoPago = r.EstadoPago,
                NombrePaciente = r.NombrePaciente
            };
        }

        public void Registrar(Factura factura)
        {
            using var db = _conexion.ObtenerConexion();
            db.Execute(@"
                INSERT INTO Factura (DNI_Paciente, FechaEmision, MontoTotal, DescripcionServicios, EstadoPago)
                VALUES (@DNI_Paciente, @FechaEmision, @MontoTotal, @DescripcionServicios, @EstadoPago)",
                new
                {
                    factura.DNI_Paciente,
                    FechaEmision = factura.FechaEmision.ToDateTime(TimeOnly.MinValue),
                    factura.MontoTotal,
                    factura.DescripcionServicios,
                    factura.EstadoPago
                });
        }

        public void ActualizarEstado(int id, string estado)
        {
            using var db = _conexion.ObtenerConexion();
            db.Execute("UPDATE Factura SET EstadoPago = @Estado WHERE ID_Factura = @ID",
                new { Estado = estado, ID = id });
        }
        public int ContarFacturasPendientes()
        {
            using var db = _conexion.ObtenerConexion();
            return db.ExecuteScalar<int>(
                "SELECT COUNT(*) FROM Factura WHERE EstadoPago = 'Pendiente'");
        }
        public void Eliminar(int id)
        {
            using var db = _conexion.ObtenerConexion();
            db.Execute("DELETE FROM Factura WHERE ID_Factura = @ID", new { ID = id });
        }
    }
}