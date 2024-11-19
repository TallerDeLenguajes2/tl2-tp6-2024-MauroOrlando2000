using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.Sqlite;

namespace tl2_tp6_2024_MauroOrlando2000.Models
{
    public class ClienteRepository : IClienteRepository
    {
        readonly string cadenaConexion = "Data Source=DB/Tienda.db;Cache=Shared";

        public List<Cliente> ObtenerClientes()
        {
            List<Cliente> lista = new List<Cliente>();
            using(SqliteConnection connection = new SqliteConnection(cadenaConexion))
            {
                var query = "SELECT * FROM Cliente;";
                connection.Open();
                var command = new SqliteCommand(query, connection);
                using(var DataReader = command.ExecuteReader())
                {
                    while(DataReader.Read())
                    {
                        int idCliente = Convert.ToInt32(DataReader["idCliente"]);
                        string nombre = Convert.ToString(DataReader["Nombre"]);
                        string email = Convert.ToString(DataReader["Email"]);
                        uint? phone = Convert.ToUInt32(DataReader["Telefono"]);
                        Cliente nuevoCliente = new Cliente(idCliente, nombre, email, phone);
                        lista.Add(nuevoCliente);
                    }
                }
            }
            return lista;
        }

        public bool CrearCliente(Cliente client)
        {
            bool anda = false;
            using(SqliteConnection connection = new SqliteConnection(cadenaConexion))
            {
                string query;
                if(client.Telefono == null)
                {
                    query = @"INSERT INTO Cliente (Nombre, Email) VALUES (@name, @mail);";
                }
                else
                {
                    query = @"INSERT INTO Cliente (Nombre, Email, Telefono) VALUES (@name, @mail, @phone);";
                }
                connection.Open();
                var command = new SqliteCommand(query, connection);
                command.Parameters.AddWithValue("@name", client.Nombre);
                command.Parameters.AddWithValue("@mail", client.Email);
                command.Parameters.AddWithValue("@phone", client.Telefono);
                anda = command.ExecuteNonQuery() > 0;
                connection.Close();
            }
            return anda;
        }

        public Cliente? Buscar(int id)
        {
            return ObtenerClientes().Find(elemento => elemento.IdCliente == id);
        }

        public bool EliminarCliente(int id)
        {
            bool anda = false;
            Cliente? aux = Buscar(id);
            if(aux != null && aux != default(Cliente))
            {
                using(SqliteConnection connection = new SqliteConnection(cadenaConexion))
                {
                    var query = @"DELETE FROM Presupuestos WHERE idCliente = @idCliente;";
                    connection.Open();
                    var command = new SqliteCommand(query, connection);
                    command.Parameters.AddWithValue("@idCliente", id);
                    command.ExecuteNonQuery();
                    connection.Close();
                }
                using(SqliteConnection connection = new SqliteConnection(cadenaConexion))
                {
                    var query = @"DELETE FROM Cliente WHERE idCliente = @idClient;";
                    connection.Open();
                    var command = new SqliteCommand(query, connection);
                    command.Parameters.AddWithValue("@idClient", id);
                    anda = command.ExecuteNonQuery() > 0;
                    connection.Close();
                }
            }
            return anda;
        }

        public bool ModificarCliente(int id, Cliente client)
        {
            bool anda = false;
            Cliente? aux = Buscar(id);
            if(aux != null && aux != default(Cliente))
            {
                using(SqliteConnection connection = new SqliteConnection(cadenaConexion))
                {
                    if(client.Email == null)
                    {
                        client.Email = aux.Email;
                    }
                    if(client.Nombre == null)
                    {
                        client.Nombre = aux.Nombre;
                    }
                    if(client.Telefono == null)
                    {
                        client.Telefono = aux.Telefono;
                    }
                    var query = @"UPDATE Cliente SET Nombre = @name, Email = @mail, Telefono = @phone WHERE idCliente = @idClient;";
                    connection.Open();
                    var command = new SqliteCommand(query, connection);
                    command.Parameters.AddWithValue("@name", client.Nombre);
                    command.Parameters.AddWithValue("@mail", client.Email);
                    command.Parameters.AddWithValue("@phone", client.Telefono);
                    command.Parameters.AddWithValue("@idClient", id);
                    anda = command.ExecuteNonQuery() > 0;
                    connection.Close();
                }
            }
            return anda;
        }
    }
}