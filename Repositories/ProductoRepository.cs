using Microsoft.Data.Sqlite;

namespace tl2_tp6_2024_MauroOrlando2000.Models
{
    public class ProductoRepository : IProductoRepository
    {
        readonly string cadenaConexion = "Data Source=DB/Tienda.db;Cache=Shared;";

        public bool CrearProducto(Producto product)
        {
            bool anda = false;
            if(product != null)
            {
                using(SqliteConnection connection = new SqliteConnection(cadenaConexion))
                {
                    if(!ObtenerProductos().Contains(product))
                    {
                        var query = @"INSERT INTO Productos (Descripcion, Precio) VALUES (@Descripcion, @Precio);";
                        connection.Open();
                        var command = new SqliteCommand(query, connection);
                        command.Parameters.AddWithValue("@Descripcion", product.Descripcion);
                        command.Parameters.AddWithValue("@Precio", product.Precio);
                        anda = command.ExecuteNonQuery() > 0;
                    }
                    connection.Close();
                }
            }
            return anda;
        }

        public bool ModificarProducto(int id, Producto product)
        {
            bool anda = false;
            Producto? aux = Buscar(id);
            if(aux != null && aux != default(Producto))
            {
                using(SqliteConnection connection = new SqliteConnection(cadenaConexion))
                {
                    var query = @"UPDATE Productos SET Descripcion = @Descripcion, Precio = @Precio WHERE idProducto = @id;";
                    connection.Open();
                    var command = new SqliteCommand(query, connection);
                    command.Parameters.AddWithValue("@Descripcion", product.Descripcion);
                    command.Parameters.AddWithValue("@Precio", product.Precio);
                    command.Parameters.AddWithValue("@id", id);
                    anda = command.ExecuteNonQuery() > 0;
                    connection.Close();
                }
            }
            return anda;
        }

        public List<Producto> ObtenerProductos()
        {
            List<Producto> lista = new List<Producto>();
            using(SqliteConnection connection = new SqliteConnection(cadenaConexion))
            {
                var query = "SELECT * FROM Productos;";
                connection.Open();
                var command = new SqliteCommand(query, connection);
                using(var DataReader = command.ExecuteReader())
                {
                    while(DataReader.Read())
                    {
                        int id = Convert.ToInt32(DataReader["idProducto"]);
                        string desc = Convert.ToString(DataReader["Descripcion"]);
                        int price = Convert.ToInt32(DataReader["Precio"]);
                        Producto nuevo = new Producto(id, desc, price);
                        lista.Add(nuevo);
                    }
                }
                connection.Close();
            }
            return lista;
        }

        public bool EliminarProducto(int id)
        {
            Producto? aux = Buscar(id);
            bool anda = false;
            if(aux != null && aux != default(Producto))
            {
                using(SqliteConnection connection = new SqliteConnection(cadenaConexion))
                {
                    var query = @"DELETE FROM Productos WHERE idProducto = @id;";
                    connection.Open();
                    var command = new SqliteCommand(query, connection);
                    command.Parameters.AddWithValue("@id", id);
                    anda = command.ExecuteNonQuery() > 0;
                    connection.Close();
                }
            }
            return anda;
        }

        public Producto? Buscar(int id)
        {
            return ObtenerProductos().Find(elemento => elemento.IdProducto == id);
        }
    }
}