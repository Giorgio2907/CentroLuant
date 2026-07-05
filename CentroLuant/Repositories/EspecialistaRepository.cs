using CentroLuant.Models;
using Dapper;

namespace CentroLuant.Repositories
{
    public class EspecialistaRepository
    {
        private readonly ConexionBD _conexion;

        public EspecialistaRepository(ConexionBD conexion)
        {
            _conexion = conexion;
        }

        public IEnumerable<Especialista> ObtenerTodos()
        {
            using var db = _conexion.ObtenerConexion();
            return db.Query<Especialista>("SELECT * FROM Especialista");
        }

        public Especialista? ObtenerPorId(int id)
        {
            using var db = _conexion.ObtenerConexion();
            return db.QueryFirstOrDefault<Especialista>(
                "SELECT * FROM Especialista WHERE ID_Especialista = @ID", new { ID = id });
        }

        public void Registrar(Especialista especialista)
        {
            using var db = _conexion.ObtenerConexion();
            db.Execute(@"
                INSERT INTO Especialista (Nombre, Apellidos, Especialidad)
                VALUES (@Nombre, @Apellidos, @Especialidad)", especialista);
        }
    }
}