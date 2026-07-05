using Microsoft.Data.SqlClient;

namespace CentroLuant.Repositories
{
    public class ConexionBD
    {
        private readonly string _connectionString;

        public ConexionBD(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("CentroLuant")!;
        }

        public SqlConnection ObtenerConexion()
        {
            return new SqlConnection(_connectionString);
        }
    }
}