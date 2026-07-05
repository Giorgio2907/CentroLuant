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
            return db.QueryFirstOrDefault<HistorialMedico>(
                "SELECT * FROM Historial_Medico WHERE DNI_Paciente = @DNI", new { DNI = dni });
        }

        public void Crear(HistorialMedico historial)
        {
            using var db = _conexion.ObtenerConexion();
            db.Execute(@"
                INSERT INTO Historial_Medico (DNI_Paciente, FechaCreacion, ObservacionesIniciales)
                VALUES (@DNI_Paciente, @FechaCreacion, @ObservacionesIniciales)", historial);
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
            return db.Query<Tratamiento>(
                "SELECT * FROM Tratamiento WHERE ID_Historial = @ID", new { ID = idHistorial });
        }

        public void RegistrarTratamiento(Tratamiento tratamiento)
        {
            using var db = _conexion.ObtenerConexion();
            db.Execute(@"
                INSERT INTO Tratamiento (ID_Historial, FechaTratamiento, Diagnostico, TipoTratamiento, Observaciones, Costo)
                VALUES (@ID_Historial, @FechaTratamiento, @Diagnostico, @TipoTratamiento, @Observaciones, @Costo)", tratamiento);
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
                WHERE ID_Tratamiento = @ID_Tratamiento", tratamiento);
        }
    }
}