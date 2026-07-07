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
                "SELECT * FROM Usuario WHERE ID_Usuario = @ID", new { ID = id });
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
                VALUES (@NombreCompleto, @UsuarioLogin, @ContrasenaHash, @Rol, 1)", usuario);
        }

        public void Actualizar(Usuario usuario)
        {
            using var db = _conexion.ObtenerConexion();
            if (string.IsNullOrEmpty(usuario.ContrasenaHash))
            {
                db.Execute(@"
                    UPDATE Usuario SET 
                        NombreCompleto = @NombreCompleto,
                        Rol = @Rol
                    WHERE ID_Usuario = @ID_Usuario", usuario);
            }
            else
            {
                db.Execute(@"
                    UPDATE Usuario SET 
                        NombreCompleto = @NombreCompleto,
                        ContrasenaHash = @ContrasenaHash,
                        Rol = @Rol
                    WHERE ID_Usuario = @ID_Usuario", usuario);
            }
        }

        public void CambiarEstado(int id)
        {
            using var db = _conexion.ObtenerConexion();
            db.Execute(@"
                UPDATE Usuario SET Activo = CASE WHEN Activo = 1 THEN 0 ELSE 1 END
                WHERE ID_Usuario = @ID", new { ID = id });
        }
    }
}