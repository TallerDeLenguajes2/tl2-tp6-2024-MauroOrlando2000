using Microsoft.Data.Sqlite;
using tl2_tp6_2024_MauroOrlando2000.Models;

namespace tl2_tp6_2024_MauroOrlando2000.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly string cadenaConexion;
        public const int Client = 1;
        public const int Admin = 2;

        public UserRepository(string connectionString)
        {
            cadenaConexion = connectionString;
        }
        public void CrearUsuario(User usuario)
        {
            try{
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
                if(!anda)
                {
                    throw new Exception("Usuario no creado");
                }
            }
            catch(SqliteException){
                throw new Exception("Error en la base de datos");
            }
            catch(Exception){
                throw;
            }
        }

        public List<User> ObtenerUsuarios()
        {
            try{
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
                if(lista.Count == 0)
                {
                    throw new Exception("No existen usuarios");
                }
                return lista;
            }
            catch(SqliteException){
                throw new Exception("Error en la base de datos");
            }
            catch(Exception){
                throw;
            }
        }

        public User Buscar(int id)
        {
            try{
                var aux = ObtenerUsuarios().Find(x => x.IdUsusario == id);
                if(aux == null)
                {
                    throw new Exception("Usuario inexistente");
                }
                return aux;
            }
            catch(SqliteException){
                throw new Exception("Error en la base de datos");
            }
            catch(Exception){
                throw;
            }
        }

        public void ModificarUsuario(int id, User usuario)
        {
            try{
                bool anda = false;
                User aux = Buscar(id);
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
                if(!anda)
                {
                    throw new Exception("Usuario no modificado");
                }
            }
            catch(SqliteException){
                throw new Exception("Error en la base de datos");
            }
            catch(Exception){
                throw;
            }
        }

        public void ModificarPassword(int id, string password)
        {
            try{
                bool anda = false;
                User aux = Buscar(id);
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
                if(!anda)
                {
                    throw new Exception("Password no modificada");
                }
            }
            catch(SqliteException){
                throw new Exception("Error enla base de datos");
            }
            catch(Exception){
                throw;
            }
        }

        public void EliminarUsuario(int id)
        {
            try{
                bool anda = false;
                User aux = Buscar(id);
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
                if(!anda)
                {
                    throw new Exception("Usuario no eliminado");
                }
            }
            catch(SqliteException){
                throw new Exception("Error en la base de datos");
            }
            catch(Exception){
                throw;
            }
        }

        public void CambiarRol(int id)
        {
            try{
                bool anda = false;
                User aux = Buscar(id);
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
                if(!anda)
                {
                    throw new Exception("Rol no cambiado");
                }
            }
            catch(SqliteException){
                throw new Exception("Error en la base de datos");
            }
            catch(Exception){
                throw;
            }
        }

        public User? Login(string usuario, string contra)
        {
            return ObtenerUsuarios().Find(x => x.Usuario == usuario && x.Password == contra);
        }
    }
}