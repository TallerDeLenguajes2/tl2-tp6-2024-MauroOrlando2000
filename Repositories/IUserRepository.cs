using tl2_tp6_2024_MauroOrlando2000.Models;

namespace tl2_tp6_2024_MauroOrlando2000.Repositories
{
    public interface IUserRepository
    {
        public void CrearUsuario(User usuario);
        public List<User> ObtenerUsuarios();
        public User Buscar(int id);
        public void ModificarUsuario(int id, User usuario);
        public void ModificarPassword(int id, string password);
        public void EliminarUsuario(int id);
        public void CambiarRol(int id);
        public User? Login(string usuario, string contra);
    }
}