using tl2_tp6_2024_MauroOrlando2000.Models;
using tl2_tp6_2024_MauroOrlando2000.ViewModels;

namespace tl2_tp6_2024_MauroOrlando2000.Repositories
{
    public interface IPresupuestoRepository
    {
        List<Presupuesto> ObtenerPresupuestos();
        void CrearPresupuesto(Presupuesto budget);
        Presupuesto Buscar(int id);
        void AgregarProducto(int idPres, int idProd, int Cant);
        void EliminarProducto(int idPres, int idProd);
        void EliminarPresupuesto(int id);
    }
}