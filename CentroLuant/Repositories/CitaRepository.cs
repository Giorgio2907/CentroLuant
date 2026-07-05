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
            var result = db.Query<dynamic>(@"
                SELECT c.*, 
                       p.Nombres + ' ' + p.Apellidos AS NombrePaciente,
                       e.Nombre + ' ' + e.Apellidos AS NombreEspecialista
                FROM Cita c
                LEFT JOIN Paciente p ON c.DNI_Paciente = p.DNI
                LEFT JOIN Especialista e ON c.ID_Especialista = e.ID_Especialista");
            return result.Select(r => new Cita
            {
                ID_Cita = r.ID_Cita,
                Fecha = DateOnly.FromDateTime((DateTime)r.Fecha),
                Hora = TimeOnly.FromTimeSpan((TimeSpan)r.Hora),
                Estado = r.Estado,
                DNI_Paciente = r.DNI_Paciente,
                ID_Especialista = r.ID_Especialista,
                NombrePaciente = r.NombrePaciente,
                NombreEspecialista = r.NombreEspecialista
            });
        }

        public IEnumerable<Cita> ObtenerPorPaciente(string dni)
        {
            using var db = _conexion.ObtenerConexion();
            var result = db.Query<dynamic>(@"
                SELECT c.*, 
                       p.Nombres + ' ' + p.Apellidos AS NombrePaciente,
                       e.Nombre + ' ' + e.Apellidos AS NombreEspecialista
                FROM Cita c
                LEFT JOIN Paciente p ON c.DNI_Paciente = p.DNI
                LEFT JOIN Especialista e ON c.ID_Especialista = e.ID_Especialista
                WHERE c.DNI_Paciente = @DNI", new { DNI = dni });
            return result.Select(r => new Cita
            {
                ID_Cita = r.ID_Cita,
                Fecha = DateOnly.FromDateTime((DateTime)r.Fecha),
                Hora = TimeOnly.FromTimeSpan((TimeSpan)r.Hora),
                Estado = r.Estado,
                DNI_Paciente = r.DNI_Paciente,
                ID_Especialista = r.ID_Especialista,
                NombrePaciente = r.NombrePaciente,
                NombreEspecialista = r.NombreEspecialista
            });
        }

        public Cita? ObtenerPorId(int id)
        {
            using var db = _conexion.ObtenerConexion();
            var r = db.QueryFirstOrDefault<dynamic>(@"
                SELECT c.*, 
                       p.Nombres + ' ' + p.Apellidos AS NombrePaciente,
                       e.Nombre + ' ' + e.Apellidos AS NombreEspecialista
                FROM Cita c
                LEFT JOIN Paciente p ON c.DNI_Paciente = p.DNI
                LEFT JOIN Especialista e ON c.ID_Especialista = e.ID_Especialista
                WHERE c.ID_Cita = @ID", new { ID = id });
            if (r == null) return null;
            return new Cita
            {
                ID_Cita = r.ID_Cita,
                Fecha = DateOnly.FromDateTime((DateTime)r.Fecha),
                Hora = TimeOnly.FromTimeSpan((TimeSpan)r.Hora),
                Estado = r.Estado,
                DNI_Paciente = r.DNI_Paciente,
                ID_Especialista = r.ID_Especialista,
                NombrePaciente = r.NombrePaciente,
                NombreEspecialista = r.NombreEspecialista
            };
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
                new
                {
                    ID = idEspecialista,
                    Fecha = fecha.ToDateTime(TimeOnly.MinValue),
                    Hora = hora.ToTimeSpan()
                });
            return count == 0;
        }

        public void Registrar(Cita cita)
        {
            using var db = _conexion.ObtenerConexion();
            db.Execute(@"
                INSERT INTO Cita (Fecha, Hora, Estado, DNI_Paciente, ID_Especialista)
                VALUES (@Fecha, @Hora, @Estado, @DNI_Paciente, @ID_Especialista)",
                new
                {
                    Fecha = cita.Fecha.ToDateTime(TimeOnly.MinValue),
                    Hora = cita.Hora.ToTimeSpan(),
                    cita.Estado,
                    cita.DNI_Paciente,
                    cita.ID_Especialista
                });
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
                new
                {
                    Fecha = nuevaFecha.ToDateTime(TimeOnly.MinValue),
                    Hora = nuevaHora.ToTimeSpan(),
                    ID = id
                });
        }
    }
}