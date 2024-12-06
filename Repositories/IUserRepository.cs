using tl2_tp6_2024_MauroOrlando2000.Models;

namespace tl2_tp6_2024_MauroOrlando2000.Repositories
{
    public interface IUserRepository
    {
        public bool CrearUsuario(User usuario);
        public List<User> ObtenerUsuarios();
        public User? Buscar(int id);
        public bool ModificarUsuario(int id, User usuario);
        public bool ModificarPassword(int id, string password);
        public bool EliminarUsuario(int id);
        public bool CambiarRol(int id);
        public bool Confirmar(User usuario);
    }
}