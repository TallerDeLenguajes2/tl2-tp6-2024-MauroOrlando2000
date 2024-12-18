using tl2_tp6_2024_MauroOrlando2000.Models;
using tl2_tp6_2024_MauroOrlando2000.ViewModels;

namespace tl2_tp6_2024_MauroOrlando2000.Repositories
{
    public interface IPresupuestoRepository
    {
        List<Presupuesto> ObtenerPresupuestos();
        bool CrearPresupuesto(Presupuesto budget);
        Presupuesto? Buscar(int id);
        bool AgregarProducto(AgregarProductoViewModel detalle);
        bool EliminarProducto(int idPres, int idProd);
        bool EliminarPresupuesto(int id);
    }
}