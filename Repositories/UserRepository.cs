using Microsoft.Data.Sqlite;
using tl2_tp6_2024_MauroOrlando2000.Models;

namespace tl2_tp6_2024_MauroOrlando2000.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly string cadenaConexion = "Data Source=DB/Tienda.db;Cache=Shared;";
        public const int Client = 1;
        public const int Admin = 2;
        public bool CrearUsuario(User usuario)
        {
            bool anda = false;
            using(SqliteConnection connection = new SqliteConnection(cadenaConexion))
            {
                var query = @"INSERT INTO Usuarios (Nombre, Usuario, Password, Rol) VALUES (@name, @user, @contra, @rol);";
                connection.Open();
                var command = new SqliteCommand(query, connection);
                command.Parameters.AddWithValue("@name", usuario.Nombre);
                command.Parameters.AddWithValue("@user", usuario.Usuario);
                command.Parameters.AddWithValue("@contra", usuario.Password);
                command.Parameters.AddWithValue("@rol", Client);
                anda = command.ExecuteNonQuery() > 0;
                connection.Close();
            }
            return anda;
        }

        public List<User> ObtenerUsuarios()
        {
            List<User> lista = new List<User>();
            using(SqliteConnection connection = new SqliteConnection(cadenaConexion))
            {
                var query = "SELECT * FROM Usuarios;";
                connection.Open();
                var command = new SqliteCommand(query, connection);
                using(var DataReader = command.ExecuteReader())
                {
                    while(DataReader.Read())
                    {
                        int id = Convert.ToInt32(DataReader["IdUsuario"]);
                        string name = Convert.ToString(DataReader["Nombre"]);
                        string user = Convert.ToString(DataReader["Usuario"]);
                        string pass = Convert.ToString(DataReader["Password"]);
                        int role = Convert.ToInt32(DataReader["Rol"]);
                        User nuevo = new User(id, name, user, pass, role);
                        lista.Add(nuevo);
                    }
                }
                connection.Close();
            }
            return lista;
        }

        public User? Buscar(int id)
        {
            return ObtenerUsuarios().Find(x => x.IdUsusario == id);
        }

        public bool ModificarUsuario(int id, User usuario)
        {
            bool anda = false;
            User? aux = Buscar(id);
            if(aux != null && aux != default(User) && usuario != null)
            {
                using(SqliteConnection connection = new SqliteConnection(cadenaConexion))
                {
                    var query = @"UPDATE Usuarios SET Nombre = @name, Usuario = @username WHERE IdUsuario = @iduser;";
                    connection.Open();
                    var command = new SqliteCommand(query, connection);
                    command.Parameters.AddWithValue("@name", usuario.Nombre);
                    command.Parameters.AddWithValue("@username", usuario.Usuario);
                    command.Parameters.AddWithValue("@iduser", id);
                    anda = command.ExecuteNonQuery() > 0;
                    connection.Close();
                }
            }
            return anda;
        }

        public bool ModificarPassword(int id, string password)
        {
            bool anda = false;
            User? aux = Buscar(id);
            if(aux != null && aux != default(User) && password != null)
            {
                using(SqliteConnection connection = new SqliteConnection(cadenaConexion))
                {
                    var query = @"UPDATE Usuarios SET Password = @pass WHERE IdUsuario = @iduser;";
                    connection.Open();
                    var command = new SqliteCommand(query, connection);
                    command.Parameters.AddWithValue("@pass", password);
                    command.Parameters.AddWithValue("@iduser", id);
                    anda = command.ExecuteNonQuery() > 0;
                    connection.Close();
                }
            }
            return anda;
        }

        public bool EliminarUsuario(int id)
        {
            bool anda = false;
            User? aux = Buscar(id);
            if(aux != null && aux != default(User))
            {
                using(SqliteConnection connection = new SqliteConnection(cadenaConexion))
                {
                    var query = @"DELETE FROM Usuarios WHERE IdUsuario = @iduser;";
                    connection.Open();
                    var command = new SqliteCommand(query, connection);
                    command.Parameters.AddWithValue("@iduser", id);
                    anda = command.ExecuteNonQuery() > 0;
                    connection.Close();
                }
            }
            return anda;
        }

        public bool CambiarRol(int id)
        {
            bool anda = false;
            User? aux = Buscar(id);
            if(aux != null && aux != default(User))
            {
                using(SqliteConnection connection = new SqliteConnection(cadenaConexion))
                {
                    string query = @"UPDATE Usuarios SET Rol = @rol WHERE IdUsuario = @iduser;";
                    connection.Open();
                    var command = new SqliteCommand(query, connection);
                    if(aux.Rol == Client)
                    {
                        command.Parameters.AddWithValue("@rol", Admin);
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@rol", Client);
                    }
                    command.Parameters.AddWithValue("@iduser", id);
                    anda = command.ExecuteNonQuery() > 0;
                    connection.Close();
                }
            }
            return anda;
        }

        public User? Login(string usuario, string contra)
        {
            return ObtenerUsuarios().Find(x => x.Usuario == usuario && x.Password == contra);
        }
    }
}