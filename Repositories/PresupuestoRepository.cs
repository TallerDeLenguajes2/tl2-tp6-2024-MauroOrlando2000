using Microsoft.Data.Sqlite;
using tl2_tp6_2024_MauroOrlando2000.Models;
using tl2_tp6_2024_MauroOrlando2000.ViewModels;

namespace tl2_tp6_2024_MauroOrlando2000.Repositories
{
    public class PresupuestoRepository : IPresupuestoRepository
    {
        private readonly string cadenaConexion;

        public PresupuestoRepository(string connectionString)
        {
            cadenaConexion = connectionString;
        }
        public void CrearPresupuesto(Presupuesto budget)
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
                    IProductoRepository repositorioProductos = new ProductoRepository(cadenaConexion);
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

        public Presupuesto Buscar(int id)
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

        public void AgregarProducto(int idPres, int idProd, int Cant)
        {
            try{
                Presupuesto aux = Buscar(idPres);
                Producto auxProd = new ProductoRepository(cadenaConexion).Buscar(idProd);
                bool anda = false;
                if(aux != null && auxProd != null)
                {
                    using(SqliteConnection connection = new SqliteConnection(cadenaConexion))
                    {
                        var query = @"INSERT INTO PresupuestosDetalle (idPresupuesto, idProducto, Cantidad) VALUES (@idPres, @idProd, @cant);";
                        connection.Open();
                        var command = new SqliteCommand(query, connection);
                        command.Parameters.AddWithValue("@idPres", idPres);
                        command.Parameters.AddWithValue("@idProd", idProd);
                        command.Parameters.AddWithValue("@cant", Cant);
                        anda = command.ExecuteNonQuery() > 0;
                        connection.Close();
                    }
                }
                if(!anda)
                {
                    throw new Exception("Producto no agregado");
                }
            }
            catch(SqliteException){
                throw new Exception("Error en la base de datos");
            }
            catch(Exception){
                throw;
            }
        }

        public void EliminarProducto(int idPres, int idProd)
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
            }
            catch(SqliteException){
                throw new Exception("Error en la base de datos");
            }
            catch(Exception)
            {
                throw;
            }
        }

        public void EliminarPresupuesto(int id)
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