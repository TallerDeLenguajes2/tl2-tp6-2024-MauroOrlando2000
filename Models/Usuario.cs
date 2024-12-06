namespace tl2_tp6_2024_MauroOrlando2000.Models
{
    public class User
    {
        private int idUsusario;
        private string nombre;
        private string usuario;
        private string password;
        private int rol;

        public int IdUsusario { get => idUsusario; }
        public string Nombre { get => nombre; set => nombre = value; }
        public string Usuario { get => usuario; set => usuario = value; }
        public string Password { get => password; }
        public int Rol { get => rol; }

        public User(int id, string name, string user, string pass, int role)
        {
            idUsusario = id;
            nombre = name;
            usuario = user;
            password = pass;
            rol = role;
        }
    }
}