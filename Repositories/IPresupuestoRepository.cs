using tl2_tp6_2024_MauroOrlando2000.Models;

namespace tl2_tp6_2024_MauroOrlando2000.Repositories
{
    public interface IPresupuestoRepository
    {
        List<Presupuesto> ObtenerPresupuestos();
        bool CrearPresupuesto(Presupuesto budget);
        Presupuesto? Buscar(int id);
        bool AgregarProducto(int idPres, PresupuestoDetalle detalle);
        bool EliminarPresupuesto(int id);
        bool EliminarProductoDetalle(int idPres, int idProd);
    }
}