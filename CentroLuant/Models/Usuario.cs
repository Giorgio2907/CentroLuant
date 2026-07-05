namespace CentroLuant.Models
{
    public class Usuario
    {
        public int ID_Usuario { get; set; }
        public string NombreCompleto { get; set; } = string.Empty;
        public string UsuarioLogin { get; set; } = string.Empty;
        public string ContrasenaHash { get; set; } = string.Empty;
        public string Rol { get; set; } = string.Empty;
        public bool Activo { get; set; } = true;
    }
}