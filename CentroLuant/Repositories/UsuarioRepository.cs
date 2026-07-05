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

        public Usuario? ObtenerPorId(int id)
        {
            using var db = _conexion.ObtenerConexion();
            return db.QueryFirstOrDefault<Usuario>(
                "SELECT * FROM Usuario WHERE ID_Usuario = @Id",
                new { Id = id });
        }

        public IEnumerable<Usuario> ObtenerTodos()
        {
            using var db = _conexion.ObtenerConexion();
            return db.Query<Usuario>("SELECT * FROM Usuario ORDER BY NombreCompleto");
        }

        public void Registrar(Usuario usuario)
        {
            using var db = _conexion.ObtenerConexion();
            db.Execute(@"
                INSERT INTO Usuario (NombreCompleto, UsuarioLogin, ContrasenaHash, Rol, Activo)
                VALUES (@NombreCompleto, @UsuarioLogin, @ContrasenaHash, @Rol, @Activo)", usuario);
        }

        public void Actualizar(Usuario usuario)
        {
            using var db = _conexion.ObtenerConexion();

            // Si no se escribió una nueva contraseña, no la tocamos
            if (!string.IsNullOrWhiteSpace(usuario.ContrasenaHash))
            {
                db.Execute(@"
                    UPDATE Usuario
                    SET NombreCompleto = @NombreCompleto,
                        UsuarioLogin   = @UsuarioLogin,
                        ContrasenaHash = @ContrasenaHash,
                        Rol            = @Rol
                    WHERE ID_Usuario = @ID_Usuario", usuario);
            }
            else
            {
                db.Execute(@"
                    UPDATE Usuario
                    SET NombreCompleto = @NombreCompleto,
                        UsuarioLogin   = @UsuarioLogin,
                        Rol            = @Rol
                    WHERE ID_Usuario = @ID_Usuario", usuario);
            }
        }

        public void CambiarEstado(int id, bool activo)
        {
            using var db = _conexion.ObtenerConexion();
            db.Execute(
                "UPDATE Usuario SET Activo = @Activo WHERE ID_Usuario = @Id",
                new { Id = id, Activo = activo });
        }

        public bool ExisteLogin(string login, int idExcluir = 0)
        {
            using var db = _conexion.ObtenerConexion();
            return db.ExecuteScalar<int>(
                "SELECT COUNT(1) FROM Usuario WHERE UsuarioLogin = @Login AND ID_Usuario <> @Id",
                new { Login = login, Id = idExcluir }) > 0;
        }
    }
}