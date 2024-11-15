using Microsoft.Data.Sqlite;
using tl2_tp6_2024_MauroOrlando2000.Models;

namespace tl2_tp6_2024_MauroOrlando2000.Repositories
{
    public class PresupuestoRepository : IPresupuestoRepository
    {
        string cadenaConexion = "Data Source=DB/Tienda.db;Cache=Shared";

        public bool CrearPresupuesto(Presupuesto budget)
        {
            bool anda = false;
            if(budget != null)
            {
                using(SqliteConnection connection = new SqliteConnection(cadenaConexion))
                {
                    connection.Open();
                    var query = @"INSERT INTO Presupuestos (NombreDestinatario, FechaCreacion) VALUES (@NombreDestinatario, @FechaCreacion);";
                    var command = new SqliteCommand(query, connection);
                    command.Parameters.AddWithValue("@NombreDestinatario", budget.NombreDestinatario);
                    command.Parameters.AddWithValue("@FechaCreacion", budget.FechaCreacion);
                    anda = command.ExecuteNonQuery() > 0;
                    connection.Close();
                }
            }
            return anda;
        }

        public List<Presupuesto> ObtenerPresupuestos()
        {
            List<Presupuesto> lista = new List<Presupuesto>();
            using(SqliteConnection connection = new SqliteConnection(cadenaConexion))
            {
                var query = "SELECT * FROM Presupuestos";
                connection.Open();
                var command = new SqliteCommand(query, connection);
                using(var DataReader = command.ExecuteReader())
                {
                    while(DataReader.Read())
                    {
                        int idPres = Convert.ToInt32(DataReader["idPresupuesto"]);
                        string nombre = Convert.ToString(DataReader["NombreDestinatario"]);
                        string fecha = Convert.ToString(DataReader["FechaCreacion"]);
                        Presupuesto nuevoPres = new Presupuesto(idPres, nombre);
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

        public bool AgregarProducto(int idPres, PresupuestoDetalle detalle)
        {
            Presupuesto? aux = Buscar(idPres);
            Producto? auxProd = new ProductoRepository().ObtenerProductos().Find(x => x.IdProducto == detalle.IDProducto);
            bool anda = false;
            if(aux != null && aux != default(Presupuesto) && auxProd != null && auxProd != default(Producto))
            {
                using(SqliteConnection connection = new SqliteConnection(cadenaConexion))
                {
                    var query = @"INSERT INTO PresupuestosDetalle (idPresupuesto, idProducto, Cantidad) VALUES (@idPres, @idProd, @cant);";
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

        public bool ModificarPresupuesto(int id, Presupuesto budget)
        {
            bool anda = false;
            Presupuesto aux = Buscar(id);
            if(aux != null && aux != default(Presupuesto))
            {
                using(SqliteConnection connection = new SqliteConnection(cadenaConexion))
                {
                    var query = @"UPDATE Presupuestos SET NombreDestinatario = @Nombre WHERE idPresupuesto = @id";
                    connection.Open();
                    var command = new SqliteCommand(query, connection);
                    command.Parameters.AddWithValue("@Nombre", budget.NombreDestinatario);
                    command.Parameters.AddWithValue("@id", id);
                    anda = command.ExecuteNonQuery() > 0;
                    connection.Close();
                }
            }
            return anda;
        }
    }
}