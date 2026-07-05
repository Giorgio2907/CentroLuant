using CentroLuant.Models;
using Dapper;

namespace CentroLuant.Repositories
{
    public class UsuarioRepository
    {
        private readonly ConexionBD _conexion;

        public UsuarioRepository(ConexionBD conexion)
        {
            _conexion = conexion;
        }

        public Usuario? ObtenerPorLogin(string login)
        {
            using var db = _conexion.ObtenerConexion();
            return db.QueryFirstOrDefault<Usuario>(
                "SELECT * FROM Usuario WHERE UsuarioLogin = @Login AND Activo = 1",
                new { Login = login });
        }

        public IEnumerable<Usuario> ObtenerTodos()
        {
            using var db = _conexion.ObtenerConexion();
            return db.Query<Usuario>("SELECT * FROM Usuario");
        }

        public void Registrar(Usuario usuario)
        {
            using var db = _conexion.ObtenerConexion();
            db.Execute(@"
                INSERT INTO Usuario (NombreCompleto, UsuarioLogin, ContrasenaHash, Rol, Activo)
                VALUES (@NombreCompleto, @UsuarioLogin, @ContrasenaHash, @Rol, @Activo)", usuario);
        }
    }
}