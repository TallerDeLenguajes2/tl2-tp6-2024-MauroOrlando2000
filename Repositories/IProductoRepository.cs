using tl2_tp6_2024_MauroOrlando2000.Models;

namespace tl2_tp6_2024_MauroOrlando2000.Repositories
{
    public interface IProductoRepository
    {
        List<Producto> ObtenerProductos();
        Producto Buscar(int id);
        void CrearProducto(Producto product);
        void ModificarProducto(int id, Producto product);
        void EliminarProducto(int id);
    }
}