using CentroLuant.Models;
using Dapper;

namespace CentroLuant.Repositories
{
    public class PacienteRepository
    {
        private readonly ConexionBD _conexion;

        public PacienteRepository(ConexionBD conexion)
        {
            _conexion = conexion;
        }

        public IEnumerable<Paciente> ObtenerTodos()
        {
            using var db = _conexion.ObtenerConexion();
            return db.Query<Paciente>("SELECT * FROM Paciente");
        }

        public Paciente? ObtenerPorDNI(string dni)
        {
            using var db = _conexion.ObtenerConexion();
            return db.QueryFirstOrDefault<Paciente>(
                "SELECT * FROM Paciente WHERE DNI = @DNI", new { DNI = dni });
        }

        public void Registrar(Paciente paciente)
        {
            using var db = _conexion.ObtenerConexion();
            db.Execute(@"
                INSERT INTO Paciente (DNI, Nombres, Apellidos, FechaNacimiento, Direccion, Telefono, CorreoElectronico, Genero)
                VALUES (@DNI, @Nombres, @Apellidos, @FechaNacimiento, @Direccion, @Telefono, @CorreoElectronico, @Genero)", paciente);
        }

        public void Actualizar(Paciente paciente)
        {
            using var db = _conexion.ObtenerConexion();
            db.Execute(@"
                UPDATE Paciente SET 
                    Nombres = @Nombres,
                    Apellidos = @Apellidos,
                    FechaNacimiento = @FechaNacimiento,
                    Direccion = @Direccion,
                    Telefono = @Telefono,
                    CorreoElectronico = @CorreoElectronico,
                    Genero = @Genero
                WHERE DNI = @DNI", paciente);
        }

        public void Eliminar(string dni)
        {
            using var db = _conexion.ObtenerConexion();
            db.Execute("DELETE FROM Paciente WHERE DNI = @DNI", new { DNI = dni });
        }
    }
}
