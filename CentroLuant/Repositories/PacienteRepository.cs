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
            var result = db.Query<dynamic>("SELECT * FROM Paciente");
            return result.Select(r => new Paciente
            {
                DNI = r.DNI,
                Nombres = r.Nombres,
                Apellidos = r.Apellidos,
                FechaNacimiento = r.FechaNacimiento != null ?
                    DateOnly.FromDateTime((DateTime)r.FechaNacimiento) : null,
                Direccion = r.Direccion,
                Telefono = r.Telefono,
                CorreoElectronico = r.CorreoElectronico,
                Genero = r.Genero
            });
        }

        public Paciente? ObtenerPorDNI(string dni)
        {
            using var db = _conexion.ObtenerConexion();
            var r = db.QueryFirstOrDefault<dynamic>(
                "SELECT * FROM Paciente WHERE DNI = @DNI", new { DNI = dni });
            if (r == null) return null;
            return new Paciente
            {
                DNI = r.DNI,
                Nombres = r.Nombres,
                Apellidos = r.Apellidos,
                FechaNacimiento = r.FechaNacimiento != null ?
                    DateOnly.FromDateTime((DateTime)r.FechaNacimiento) : null,
                Direccion = r.Direccion,
                Telefono = r.Telefono,
                CorreoElectronico = r.CorreoElectronico,
                Genero = r.Genero
            };
        }

        public void Registrar(Paciente paciente)
        {
            using var db = _conexion.ObtenerConexion();
            db.Execute(@"
                INSERT INTO Paciente (DNI, Nombres, Apellidos, FechaNacimiento, Direccion, Telefono, CorreoElectronico, Genero)
                VALUES (@DNI, @Nombres, @Apellidos, @FechaNacimiento, @Direccion, @Telefono, @CorreoElectronico, @Genero)",
                new
                {
                    paciente.DNI,
                    paciente.Nombres,
                    paciente.Apellidos,
                    FechaNacimiento = paciente.FechaNacimiento.HasValue ?
                        paciente.FechaNacimiento.Value.ToDateTime(TimeOnly.MinValue) : (DateTime?)null,
                    paciente.Direccion,
                    paciente.Telefono,
                    paciente.CorreoElectronico,
                    paciente.Genero
                });
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
                WHERE DNI = @DNI",
                new
                {
                    paciente.DNI,
                    paciente.Nombres,
                    paciente.Apellidos,
                    FechaNacimiento = paciente.FechaNacimiento.HasValue ?
                        paciente.FechaNacimiento.Value.ToDateTime(TimeOnly.MinValue) : (DateTime?)null,
                    paciente.Direccion,
                    paciente.Telefono,
                    paciente.CorreoElectronico,
                    paciente.Genero
                });
        }

        public void Eliminar(string dni)
        {
            using var db = _conexion.ObtenerConexion();
            db.Execute("DELETE FROM Paciente WHERE DNI = @DNI", new { DNI = dni });
        }
    }
}