using CentroLuant.Models;
using Dapper;

namespace CentroLuant.Repositories
{
    public class HistorialRepository
    {
        private readonly ConexionBD _conexion;

        public HistorialRepository(ConexionBD conexion)
        {
            _conexion = conexion;
        }

        public HistorialMedico? ObtenerPorPaciente(string dni)
        {
            using var db = _conexion.ObtenerConexion();
            var r = db.QueryFirstOrDefault<dynamic>(
                "SELECT * FROM Historial_Medico WHERE DNI_Paciente = @DNI", new { DNI = dni });
            if (r == null) return null;
            return new HistorialMedico
            {
                ID_Historial = r.ID_Historial,
                DNI_Paciente = r.DNI_Paciente,
                FechaCreacion = DateOnly.FromDateTime((DateTime)r.FechaCreacion),
                ObservacionesIniciales = r.ObservacionesIniciales
            };
        }

        public void Crear(HistorialMedico historial)
        {
            using var db = _conexion.ObtenerConexion();
            db.Execute(@"
                INSERT INTO Historial_Medico (DNI_Paciente, FechaCreacion, ObservacionesIniciales)
                VALUES (@DNI_Paciente, @FechaCreacion, @ObservacionesIniciales)",
                new
                {
                    historial.DNI_Paciente,
                    FechaCreacion = historial.FechaCreacion.ToDateTime(TimeOnly.MinValue),
                    historial.ObservacionesIniciales
                });
        }

        public void Actualizar(HistorialMedico historial)
        {
            using var db = _conexion.ObtenerConexion();
            db.Execute(@"
                UPDATE Historial_Medico SET ObservacionesIniciales = @ObservacionesIniciales
                WHERE ID_Historial = @ID_Historial", historial);
        }

        public IEnumerable<Tratamiento> ObtenerTratamientos(int idHistorial)
        {
            using var db = _conexion.ObtenerConexion();
            var result = db.Query<dynamic>(
                "SELECT * FROM Tratamiento WHERE ID_Historial = @ID", new { ID = idHistorial });
            return result.Select(r => new Tratamiento
            {
                ID_Tratamiento = r.ID_Tratamiento,
                ID_Historial = r.ID_Historial,
                FechaTratamiento = DateOnly.FromDateTime((DateTime)r.FechaTratamiento),
                Diagnostico = r.Diagnostico,
                TipoTratamiento = r.TipoTratamiento,
                Observaciones = r.Observaciones,
                Costo = r.Costo
            });
        }

        public void RegistrarTratamiento(Tratamiento tratamiento)
        {
            using var db = _conexion.ObtenerConexion();
            db.Execute(@"
                INSERT INTO Tratamiento (ID_Historial, FechaTratamiento, Diagnostico, TipoTratamiento, Observaciones, Costo)
                VALUES (@ID_Historial, @FechaTratamiento, @Diagnostico, @TipoTratamiento, @Observaciones, @Costo)",
                new
                {
                    tratamiento.ID_Historial,
                    FechaTratamiento = tratamiento.FechaTratamiento.ToDateTime(TimeOnly.MinValue),
                    tratamiento.Diagnostico,
                    tratamiento.TipoTratamiento,
                    tratamiento.Observaciones,
                    tratamiento.Costo
                });
        }

        public void ActualizarTratamiento(Tratamiento tratamiento)
        {
            using var db = _conexion.ObtenerConexion();
            db.Execute(@"
                UPDATE Tratamiento SET
                    FechaTratamiento = @FechaTratamiento,
                    Diagnostico = @Diagnostico,
                    TipoTratamiento = @TipoTratamiento,
                    Observaciones = @Observaciones,
                    Costo = @Costo
                WHERE ID_Tratamiento = @ID_Tratamiento",
                new
                {
                    FechaTratamiento = tratamiento.FechaTratamiento.ToDateTime(TimeOnly.MinValue),
                    tratamiento.Diagnostico,
                    tratamiento.TipoTratamiento,
                    tratamiento.Observaciones,
                    tratamiento.Costo,
                    tratamiento.ID_Tratamiento
                });
        }
    }
}