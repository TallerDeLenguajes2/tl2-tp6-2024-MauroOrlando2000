using Microsoft.Data.Sqlite;
using tl2_tp6_2024_MauroOrlando2000.Models;
using tl2_tp6_2024_MauroOrlando2000.ViewModels;

namespace tl2_tp6_2024_MauroOrlando2000.Repositories
{
    public class PresupuestoRepository : IPresupuestoRepository
    {
        string cadenaConexion = "Data Source=DB/Tienda.db;Cache=Shared";

        public bool CrearPresupuesto(Presupuesto budget)
        {
            try{
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
                if(anda == false)
                {
                    throw new Exception("Producto no agregado");
                }
                return anda;
            }
            catch(SqliteException){
                throw new Exception("Error en la base de datos");
            }
            catch (Exception){
                throw;
            }
        }

        public List<Presupuesto> ObtenerPresupuestos()
        {
            try{
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
                if(lista.Count == 0)
                {
                    throw new Exception("Lista de productos vacia");
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

        public Presupuesto? Buscar(int id)
        {
            try{
                var aux = ObtenerPresupuestos().Find(x => x.IdPresupuesto == id);
                if(aux == null)
                {
                    throw new Exception("Presupuesto inexistente");
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

        public bool AgregarProducto(AgregarProductoViewModel detalle)
        {
            try{
                Presupuesto? aux = Buscar(detalle.IdPresupuesto);
                Producto? auxProd = new ProductoRepository().Buscar(detalle.IdProducto);
                bool anda = false;
                if(aux != null && aux != default(Presupuesto) && auxProd != null && auxProd != default(Producto))
                {
                    using(SqliteConnection connection = new SqliteConnection(cadenaConexion))
                    {
                        var query = @"INSERT INTO PresupuestosDetalle (idPresupuesto, idProducto, Cantidad) VALUES (@idPres, @idProd, @cant);";
                        connection.Open();
                        var command = new SqliteCommand(query, connection);
                        command.Parameters.AddWithValue("@idPres", detalle.IdPresupuesto);
                        command.Parameters.AddWithValue("@idProd", detalle.IdProducto);
                        command.Parameters.AddWithValue("@cant", detalle.Cantidad);
                        anda = command.ExecuteNonQuery() > 0;
                        connection.Close();
                    }
                }
                if(!anda)
                {
                    throw new Exception("Producto no agregado");
                }
                return anda;
            }
            catch(SqliteException){
                throw new Exception("Error en la base de datos");
            }
            catch(Exception){
                throw;
            }
        }

        public bool EliminarProducto(int idPres, int idProd)
        {
            try{
                bool anda = false;
                Presupuesto? aux = Buscar(idPres);
                if(aux != null)
                {
                    using(SqliteConnection connection = new SqliteConnection(cadenaConexion))
                    {
                        var query = @"DELETE FROM PresupuestosDetalle WHERE idPresupuesto = @idpres AND idProducto = @idprod;";
                        connection.Open();
                        var command = new SqliteCommand(query, connection);
                        command.Parameters.AddWithValue("@idpres", idPres);
                        command.Parameters.AddWithValue("@idprod", idProd);
                        anda = command.ExecuteNonQuery() > 0;
                        connection.Close();
                    }
                }
                if(!anda)
                {
                    throw new Exception("Producto no eliminado");
                }
                return anda;
            }
            catch(SqliteException){
                throw new Exception("Error en la base de datos");
            }
            catch(Exception)
            {
                throw;
            }
        }

        public bool EliminarPresupuesto(int id)
        {
            try{
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
                if(!anda)
                {
                    throw new Exception("Presupuesto no eliminado");
                }
                return anda;
            }
            catch(SqliteException){
                throw new Exception("Error en la base de datos");
            }
            catch(Exception){
                throw;
            }
        }
    }
}