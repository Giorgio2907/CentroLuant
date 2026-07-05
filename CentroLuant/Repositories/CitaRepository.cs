using CentroLuant.Models;
using Dapper;

namespace CentroLuant.Repositories
{
    public class CitaRepository
    {
        private readonly ConexionBD _conexion;

        public CitaRepository(ConexionBD conexion)
        {
            _conexion = conexion;
        }

        public IEnumerable<Cita> ObtenerTodas()
        {
            using var db = _conexion.ObtenerConexion();
            return db.Query<Cita>(@"
                SELECT c.*, 
                       p.Nombres + ' ' + p.Apellidos AS NombrePaciente,
                       e.Nombre + ' ' + e.Apellidos AS NombreEspecialista
                FROM Cita c
                LEFT JOIN Paciente p ON c.DNI_Paciente = p.DNI
                LEFT JOIN Especialista e ON c.ID_Especialista = e.ID_Especialista");
        }

        public IEnumerable<Cita> ObtenerPorPaciente(string dni)
        {
            using var db = _conexion.ObtenerConexion();
            return db.Query<Cita>(@"
                SELECT c.*, 
                       p.Nombres + ' ' + p.Apellidos AS NombrePaciente,
                       e.Nombre + ' ' + e.Apellidos AS NombreEspecialista
                FROM Cita c
                LEFT JOIN Paciente p ON c.DNI_Paciente = p.DNI
                LEFT JOIN Especialista e ON c.ID_Especialista = e.ID_Especialista
                WHERE c.DNI_Paciente = @DNI", new { DNI = dni });
        }

        public Cita? ObtenerPorId(int id)
        {
            using var db = _conexion.ObtenerConexion();
            return db.QueryFirstOrDefault<Cita>(@"
                SELECT c.*, 
                       p.Nombres + ' ' + p.Apellidos AS NombrePaciente,
                       e.Nombre + ' ' + e.Apellidos AS NombreEspecialista
                FROM Cita c
                LEFT JOIN Paciente p ON c.DNI_Paciente = p.DNI
                LEFT JOIN Especialista e ON c.ID_Especialista = e.ID_Especialista
                WHERE c.ID_Cita = @ID", new { ID = id });
        }

        public bool VerificarDisponibilidad(int idEspecialista, DateOnly fecha, TimeOnly hora)
        {
            using var db = _conexion.ObtenerConexion();
            var count = db.ExecuteScalar<int>(@"
                SELECT COUNT(*) FROM Cita 
                WHERE ID_Especialista = @ID 
                AND Fecha = @Fecha 
                AND Hora = @Hora
                AND Estado != 'Cancelada'",
                new { ID = idEspecialista, Fecha = fecha, Hora = hora });
            return count == 0;
        }

        public void Registrar(Cita cita)
        {
            using var db = _conexion.ObtenerConexion();
            db.Execute(@"
                INSERT INTO Cita (Fecha, Hora, Estado, DNI_Paciente, ID_Especialista)
                VALUES (@Fecha, @Hora, @Estado, @DNI_Paciente, @ID_Especialista)", cita);
        }

        public void Cancelar(int id)
        {
            using var db = _conexion.ObtenerConexion();
            db.Execute("UPDATE Cita SET Estado = 'Cancelada' WHERE ID_Cita = @ID", new { ID = id });
        }

        public void Reprogramar(int id, DateOnly nuevaFecha, TimeOnly nuevaHora)
        {
            using var db = _conexion.ObtenerConexion();
            db.Execute(@"
                UPDATE Cita SET Fecha = @Fecha, Hora = @Hora 
                WHERE ID_Cita = @ID",
                new { Fecha = nuevaFecha, Hora = nuevaHora, ID = id });
        }
    }
}