using tl2_tp6_2024_MauroOrlando2000.Models;

namespace tl2_tp6_2024_MauroOrlando2000.Repositories
{
    public interface IProductoRepository
    {
        List<Producto> ObtenerProductos();
        Producto? Buscar(int id);
        bool CrearProducto(Producto product);
        bool ModificarProducto(int id, Producto product);
        bool EliminarProducto(int id);
    }
}