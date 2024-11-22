using Microsoft.Data.Sqlite;
using tl2_tp6_2024_MauroOrlando2000.Models;

namespace tl2_tp6_2024_MauroOrlando2000.Repositories
{
    public class PresupuestoRepository : IPresupuestoRepository
    {
        readonly string cadenaConexion = "Data Source=DB/Tienda.db;Cache=Shared";

        public bool CrearPresupuesto(AltaPresupuestoViewModel budget)
        {
            bool anda = false;
            if(budget != null)
            {
                Cliente aux = new ClienteRepository().Buscar(budget.IdCliente);
                if(aux != null && aux != default(Cliente))
                {
                    using(SqliteConnection connection = new SqliteConnection(cadenaConexion))
                    {
                        connection.Open();
                        var query = @"INSERT INTO Presupuestos (idCliente, FechaCreacion) VALUES (@idCliente, @FechaCreacion);";
                        var command = new SqliteCommand(query, connection);
                        command.Parameters.AddWithValue("@idCliente", budget.IdCliente);
                        command.Parameters.AddWithValue("@FechaCreacion", DateOnly.FromDateTime(DateTime.Today).ToString("o"));
                        anda = command.ExecuteNonQuery() > 0;
                        connection.Close();
                    }
                }
            }
            return anda;
        }

        public List<Presupuesto> ObtenerPresupuestos()
        {
            List<Presupuesto> lista = new List<Presupuesto>();
            using(SqliteConnection connection = new SqliteConnection(cadenaConexion))
            {
                var query = "SELECT * FROM Presupuestos INNER JOIN Cliente USING(idCliente);";
                connection.Open();
                var command = new SqliteCommand(query, connection);
                using(var DataReader = command.ExecuteReader())
                {
                    while(DataReader.Read())
                    {
                        int idCliente = Convert.ToInt32(DataReader["idCliente"]);
                        string nombre = Convert.ToString(DataReader["Nombre"]);
                        string email = Convert.ToString(DataReader["Email"]);
                        uint phone = Convert.ToUInt32(DataReader["Telefono"]);
                        Cliente nuevoCliente = new Cliente(idCliente, nombre, email, phone);
                        int idPres = Convert.ToInt32(DataReader["idPresupuesto"]);
                        string fecha = Convert.ToString(DataReader["FechaCreacion"]);
                        Presupuesto nuevoPres = new Presupuesto(idPres, nuevoCliente);
                        nuevoPres.CambiarFecha(fecha);
                        lista.Add(nuevoPres);
                    }
                }
                query = "SELECT * FROM PresupuestosDetalle;";
                command = new SqliteCommand(query, connection);
                IProductoRepository repositorioProductos = new ProductoRepository();
                using(var DataReader = command.ExecuteReader())
                {
                    while(DataReader.Read())
                    {
                        int idPres = Convert.ToInt32(DataReader["idPresupuesto"]);
                        int idProd = Convert.ToInt32(DataReader["idProducto"]);
                        int cant = Convert.ToInt32(DataReader["Cantidad"]);
                        Producto aux = repositorioProductos.Buscar(idProd);
                        Presupuesto auxPres = lista.Find(elemento => elemento.IdPresupuesto == idPres);
                        auxPres.AgregarProducto(aux, cant);
                    }
                }
                connection.Close();
            }
            return lista;
        }

        public Presupuesto? Buscar(int id)
        {
            return ObtenerPresupuestos().Find(x => x.IdPresupuesto == id);
        }

        public bool AgregarProducto(AgregarProductoViewModel detalle)
        {
            Presupuesto? aux = Buscar(detalle.IdPresupuesto);
            Producto? auxProd = new ProductoRepository().Buscar(detalle.IdProducto);
            bool anda = false;
            if(aux != null && aux != default(Presupuesto) && auxProd != null && auxProd != default(Producto) && aux.Detalle.Exists(x => x.Producto.IdProducto == detalle.IdProducto))
            {
                using(SqliteConnection connection = new SqliteConnection(cadenaConexion))
                {
                    var query = @"UPDATE PresupuestosDetalle SET Cantidad = Cantidad + @Cant WHEREidPresupuesto = @idPresu AND idProducto = @idProdu;";
                    connection.Open();
                    var command = new SqliteCommand(query, connection);
                    command.Parameters.AddWithValue("@Cant", detalle.Cantidad);
                    command.Parameters.AddWithValue("@idPresu", detalle.IdPresupuesto);
                    command.Parameters.AddWithValue("@idProdu", detalle.IdProducto);
                    anda = command.ExecuteNonQuery() > 0;
                    connection.Close();
                }
            }
            else
            {
                using(SqliteConnection connection = new SqliteConnection(cadenaConexion))
                {
                    var query = @"INSERT INTO PresupuestosDetalle (idPresupuesto, idProducto, Cantidad)VALUES (@idPres, @idProd, @cant);";
                    connection.Open();
                    var command = new SqliteCommand(query, connection);
                    command.Parameters.AddWithValue("@idPres", aux.IdPresupuesto);
                    command.Parameters.AddWithValue("@idProd", auxProd.IdProducto);
                    command.Parameters.AddWithValue("@cant", detalle.Cantidad);
                    anda = command.ExecuteNonQuery() > 0;
                    connection.Close();
                }
            }
            return anda;
        }

        public bool EliminarPresupuesto(int id)
        {
            Presupuesto? aux = Buscar(id);
            bool anda = false;
            if(aux != null && aux != default(Presupuesto))
            {
                using(SqliteConnection connection = new SqliteConnection(cadenaConexion))
                {
                    var query = @"DELETE FROM PresupuestosDetalle WHERE idPresupuesto = @id";
                    connection.Open();
                    var command = new SqliteCommand(query, connection);
                    command.Parameters.AddWithValue("@id", id);
                    command.ExecuteNonQuery();
                    connection.Close();
                }
                using(SqliteConnection connection = new SqliteConnection(cadenaConexion))
                {
                    var query = @"DELETE FROM Presupuestos WHERE idPresupuesto = @id";
                    connection.Open();
                    var command = new SqliteCommand(query, connection);
                    command.Parameters.AddWithValue("@id", id);
                    anda = command.ExecuteNonQuery() > 0;
                    connection.Close();
                }
            }
            return anda;
        }

        public bool EliminarProductoDetalle(int idPres, int idProd)
        {
            bool anda = false;
            Presupuesto? aux = Buscar(idPres);
            if(aux != null && aux != default(Presupuesto))
            {
                if(aux.Detalle.Exists(x => x.Producto.IdProducto == idProd))
                {
                    using(SqliteConnection connection = new SqliteConnection(cadenaConexion))
                    {
                        var query = @"DELETE FROM PresupuestosDetalle WHERE idPresupuesto = @idPresu AND idProducto = @idProdu;";
                        connection.Open();
                        var command = new SqliteCommand(query, connection);
                        command.Parameters.AddWithValue("@idPresu", idPres);
                        command.Parameters.AddWithValue("@idProdu", idProd);
                        anda = command.ExecuteNonQuery() > 0;
                        connection.Close();
                    }
                }
            }
            return anda;
        }
    }
}